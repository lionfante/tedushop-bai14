using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using TeduShop.Common.Exceptions;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Api
{
    [RoutePrefix("api/applicationgroup")]
    [Authorize]
    public class ApplicationGroupController : ApiControllerBase
    {
        IApplicationGroupService _applicationGroupService;
        IApplicationRoleService _applicationRoleService;
        ApplicationUserManager _userManager;
        
        public ApplicationGroupController(IApplicationGroupService applicationGroupService,
                                            IApplicationRoleService applicationRoleService,
                                            ApplicationUserManager userManager,
                                            IErrorService errorService):base(errorService)
        {
            _applicationGroupService = applicationGroupService;
            _applicationRoleService = applicationRoleService;
            _userManager = userManager;
            
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRows = 0;
                var model = _applicationGroupService.GetAll(page, pageSize, out totalRows, filter);

                totalRows = model.Count();

                var query = model.OrderByDescending(x => x.Name).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(query);

                var paginationSet = new PaginationSet<ApplicationGroupViewModel>() {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRows,
                    TotalPages = (int)Math.Ceiling((decimal)totalRows / pageSize)
                };

                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () => {
                var model = _applicationGroupService.GetAll();
                IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, modelVm);
                return response;
            });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Detail(HttpRequestMessage request, int id)
        {
            if(id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }
            ApplicationGroup group = _applicationGroupService.GetDetail(id);
            
            if (group == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No Content.");
            }
            
            var applicationGroupVm = Mapper.Map<ApplicationGroup, ApplicationGroupViewModel>(group);
            var listRoles = _applicationRoleService.GetListRoleByGroupId(id);
            var listRolesVm = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRoles);
            applicationGroupVm.Roles = listRolesVm;


            var response = request.CreateResponse(HttpStatusCode.OK, applicationGroupVm);
            return response;

        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newGroup = new ApplicationGroup();
                newGroup.Name = appGroupViewModel.Name;

                try
                {
                    var appGroup = _applicationGroupService.Add(newGroup);
                    _applicationGroupService.Save();

                    //save roles to group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _applicationRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _applicationRoleService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
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

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var appGroup = _applicationGroupService.GetDetail(appGroupViewModel.ID);
                try
                {
                    appGroup.UpdateApplicationGroup(appGroupViewModel);
                    _applicationGroupService.Update(appGroup);

                    //save roles to group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach(var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _applicationRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _applicationRoleService.Save();

                    //add role to user
                    var listRole = _applicationRoleService.GetListRoleByGroupId(appGroup.ID);
                    var listUserInGroup = _applicationGroupService.GetListUserByGroupId(appGroup.ID);

                    foreach(var user in listUserInGroup)
                    {
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach(var roleName in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                            await _userManager.AddToRoleAsync(user.Id, roleName);
                        }
                    }
                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                }
                catch(NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            var appGroup = _applicationGroupService.Delete(id);
            _applicationGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [HttpDelete]
        [Route("deletemulti")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach(var item in listItem)
                    {
                        _applicationGroupService.Delete(item);
                    }
                    _applicationGroupService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }
                return response;
            });
        }
    }
}
