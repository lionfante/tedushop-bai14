using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Api
{
    [RoutePrefix("api/applicationusers")]
    //[Authorize(Roles ="Admin")]
    public class ApplicationUserController : ApiControllerBase
    {
        private ApplicationUserManager _appUserManager;
        private IApplicationGroupService _applicationGroupService;
        private IApplicationRoleService _applicationRoleService;

        public ApplicationUserController(IApplicationGroupService applicationGroupService, 
                                            IApplicationRoleService applicationRoleService,
                                            ApplicationUserManager appUserManager , 
                                            IErrorService errorService) : base(errorService)
        {
            _appUserManager = appUserManager;
            _applicationGroupService = applicationGroupService;
            _applicationRoleService = applicationRoleService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _appUserManager.Users;
                var modelpaging = model.OrderBy(x=>x.UserName).Skip(page*pageSize).Take(pageSize);
                int totalRow = model.Count();
                int totalPages = (int)Math.Ceiling((double)totalRow / pageSize);

                var modelVm = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(modelpaging);
                PaginationSet<ApplicationUserViewModel> pageSet = new PaginationSet<ApplicationUserViewModel>()
                {
                    Items = modelVm,
                    Page = page,
                    TotalPages = totalPages,
                    TotalCount = totalRow
                };
                response = request.CreateResponse(HttpStatusCode.OK, pageSet);
                return response;
            });
        }

        [Route("add")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, ApplicationUserViewModel appUserVm)
        {
            if (ModelState.IsValid)
            {
                var model = new ApplicationUser();
                model.UpdateApplicationUser(appUserVm);
                var result = await _appUserManager.CreateAsync(model, appUserVm.Password);
                if (result.Succeeded)
                {
                    var listAppUserGroup = new List<ApplicationUserGroup>();

                    foreach(var group in appUserVm.Groups)
                    {
                        listAppUserGroup.Add(new ApplicationUserGroup
                        {
                            GroupId = group.ID,
                            UserId = model.Id
                        });

                        //add role to user
                        var listRole = _applicationRoleService.GetListRoleByGroupId(group.ID);

                        foreach(var role in listRole)
                        {
                            await _appUserManager.RemoveFromRoleAsync(model.Id, role.Name);
                            await _appUserManager.AddToRoleAsync(model.Id, role.Name);
                        }
                    }
                    _applicationGroupService.AddUserToGroups(listAppUserGroup, model.Id);
                    _applicationGroupService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, appUserVm);
                }else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
            }else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {

                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            var user = _appUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var date = Convert.ToDateTime(user.Result.BirthDay).Date;
                var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user.Result);
                applicationUserViewModel.BirthDay = date;
                var listGroup = _applicationGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
                applicationUserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }

        }
        //[Route("update")]
        //[HttpPut]
        //public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationUserViewModel appUserVm)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }else
        //    {
        //        return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
        //}
    }
}
