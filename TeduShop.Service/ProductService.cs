using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Common;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IProductService
    {
        Product Add(Product product);

        void Update(Product product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetLastest(int top);
        IEnumerable<Product> GetHotProduct(int top);

        IEnumerable<Product> GetAllByCategoryId(int categoryId);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize,string sort, out int totalRow);

        IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<string> GetListProductByName(string name);

        IEnumerable<Product> GetRelatedProduct(int id, int top);

        Product GetById(int id);

        IEnumerable<Tag> GetListTagByProductId(int id);

        Tag GetTags(string tagId);

        void IncreaseView(int id);

        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);

        bool SellProduct(int productId, int quantity);

        void Save();
    }
    public class ProductService : IProductService
    {
        //Ten bien cua class phai dat co dau gach ngang.
        private IProductRepository _productRepository;
        private IUnitOfWork _unitOfWork;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository, ITagRepository tagRepository ,IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;
            this._unitOfWork = unitOfWork;
        }
        public Product Add(Product product)
        {
            var productResult = _productRepository.Add(product);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if(_tagRepository.Count(x=>x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    //Tag co roi se khong tao.
                    //ProductTag thi luon luon se tao.
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }
            return productResult;
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetAllByCategoryId(int categoryId)
        {
            throw new Exception();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    //Tag co roi se khong tao.
                    //ProductTag thi luon luon se tao.
                    _productTagRepository.DeleteMulti(x => x.ProductID == product.ID);
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
                
            }
            
        }

        public IEnumerable<Product> GetLastest(int top)
        {
            return _productRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _productRepository.GetMulti(x => x.Status && x.HotFlag == true).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow )
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _productRepository.GetMulti(x => x.Status && x.Name.Contains(name)).Select(y=>y.Name);
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetRelatedProduct(int id, int top)
        {
            var product = _productRepository.GetSingleById(id);
            return _productRepository.GetMulti(x => x.Status && x.ID != id && x.CategoryID == product.CategoryID).Take(top);
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public void IncreaseView(int id)
        {
            var product = _productRepository.GetSingleById(id);
            if (product.ViewCount.HasValue)
            {
                product.ViewCount += 1;
            }else
            {
                product.ViewCount = 1;
            }
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var model = _productRepository.GetListProductByTag(tagId, page, pageSize, out totalRow);
            return model;
        }

        public Tag GetTags(string tagId)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }

        public bool SellProduct(int productId, int quantity)
        {
            var product = _productRepository.GetSingleById(productId);
            if (product.Quantity < quantity)
                return false;
            product.Quantity -= quantity;
            return true;

        }
    }
}
