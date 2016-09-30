using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TeduShop.Common.Exceptions;
using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Api
{
    [RoutePrefix("api/applicationrole")]
    public class ApplicationRoleController : ApiControllerBase
    {
        private IApplicationRoleService _appRoleService;
        public ApplicationRoleController(IErrorService errorService, IApplicationRoleService appRoleService) : base(errorService)
        {
            _appRoleService = appRoleService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _appRoleService.GetAll(page, pageSize, out totalRow, filter).ToList();
                var appRoleVm = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                PaginationSet<ApplicationRoleViewModel> pageSet = new PaginationSet<ApplicationRoleViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((double)totalRow / pageSize),
                    Items = appRoleVm
                };
                response = request.CreateResponse(HttpStatusCode.OK, pageSet);
                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _appRoleService.GetAll();
                IEnumerable<ApplicationRoleViewModel> appRoleVm = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(model);
                response = request.CreateResponse(HttpStatusCode.OK, appRoleVm);
                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Detail(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị");

            var model = _appRoleService.GetDetail(id);
            if(model == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Roles");
            }
            var modelVm = Mapper.Map<ApplicationRole, ApplicationRoleViewModel>(model);
            return request.CreateResponse(HttpStatusCode.OK, modelVm);
        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ApplicationRoleViewModel appRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newAppRole = new ApplicationRole();
                    newAppRole.UpdateApplicationRole(appRole);

                    _appRoleService.Add(newAppRole);
                    _appRoleService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, newAppRole);
                }
                catch(NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
                
            }else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ApplicationRoleViewModel appRoleVm)
        {
            if (ModelState.IsValid)
            {
                var model = _appRoleService.GetDetail(appRoleVm.Id);
                try
                {
                    model.UpdateApplicationRole(appRoleVm, "update");
                    _appRoleService.Update(model);
                    _appRoleService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, model);
                }
                catch(NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
                
            }else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị");

            _appRoleService.Delete(id);
            _appRoleService.Save();

            return request.CreateResponse(HttpStatusCode.OK, id);
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (string.IsNullOrEmpty(checkedList))
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(checkedList) + " không có giá trị");

                var delList = new JavaScriptSerializer().Deserialize<List<string>>(checkedList);
                foreach (var item in delList)
                {
                    _appRoleService.Delete(item);
                }
                _appRoleService.Save();

                response = request.CreateResponse(HttpStatusCode.OK, checkedList.Count());
                return response;
            });
            
        }
    }
}
