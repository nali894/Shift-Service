using ExceptionsLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ShiftService
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private Response _oResponse=new Response(ExceptionCode.Ok);
        private readonly ILogger<ServiceController> _logger;
        public ServiceController(ILogger<ServiceController> logger)
        { 
            _logger = logger;        
        }

        [HttpPost]
        [Route("AcceptService")]
        public Response AcceptService(ServiceUser oServiceUser)
        {
            try
            {                
                Update oUpdate = new Update();
                bool bUpdateUser = oUpdate.UpdateService_User(oServiceUser.intServiceID, oServiceUser.strUserCode);
          
                if (bUpdateUser)
                {
                    bool bUpdateStatus = oUpdate.UpdateStatus(oServiceUser.intServiceID, "2");

                    if (bUpdateStatus)
                    {
                        _oResponse = new Response(ExceptionCode.Accepted, true);                                           
                    }
                    else
                    {
                        _oResponse = new Response(ExceptionCode.IsNotUpdated, false);
                        _logger.LogError($"* AcceptService - Error: Service {oServiceUser.intServiceID}-{_oResponse.Code} {_oResponse.Description}");
                    }
                }
                else
                {
                    _oResponse = new Response(ExceptionCode.IsNotAssigned, false);
                    _logger.LogError($"* AcceptService - Error: Service {oServiceUser.intServiceID}-{_oResponse.Code} {_oResponse.Description}");
                }

            }
            catch (Exception ex)
            {               
                _oResponse = new Response(ExceptionCode.Exception, false);
               
                ArgumentException aex = new ArgumentException(ex.Message.ToString());
                _logger.LogCritical(aex, $"* AcceptService - Exception:Service {oServiceUser.intServiceID} | {ex.Message} | {ex.StackTrace} ");
            }

            return _oResponse;
        }

        [HttpPost]
        [Route("CreateService")]
        public Response CreateService(Service oService)
        {            
            try
            {
                Create oCreate = new Create();
                bool bCreateService = oCreate.CreateService(oService);
            
                if (bCreateService)
                {
                    _oResponse = new Response(ExceptionCode.Created, true); 
                }
                else
                {
                    _oResponse = new Response(ExceptionCode.IsNotUpdated, false);
                    _logger.LogError($"* CreateService - Error: Service [{oService.ServiceType},{oService.Address}]-{_oResponse.Code} {_oResponse.Description}");
                }
            }
            catch (Exception ex)
            {
                _oResponse = new Response(ExceptionCode.Exception, false);
               
                ArgumentException aex = new ArgumentException(ex.Message.ToString());
                _logger.LogCritical(aex, $"* CreateService - Exception:Service [{oService.ServiceType},{oService.Address}]- {ex.Message} | {ex.StackTrace} ");
               
            }           

            return _oResponse;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public Response GetAllRoles()
        {           
            try
            {              
                Read read = new Read();
                List<RoleService> lstService = read.GetAllRoles();
                _oResponse.Values = lstService;
            }
            catch (Exception ex)
            {
                _oResponse = new Response(ExceptionCode.Exception, false);
              
                ArgumentException aex = new ArgumentException(ex.Message.ToString());
                _logger.LogCritical(aex, $"* GetAllRoles - Exception: {ex.Message} | {ex.StackTrace} ");

            }

            return _oResponse;
        }

        [HttpGet]
        [Route("GetAllServices")]
        public Response GetAllServices()
        {
            try
            {               
                Read read = new Read();
                List<Service> lstService = read.GetAllServices();
                _oResponse.Values = lstService;
            }
            catch (Exception ex)
            {
                _oResponse = new Response(ExceptionCode.Exception, false);
               
                ArgumentException aex = new ArgumentException(ex.Message.ToString());
                _logger.LogCritical(aex, $"* GetAllServices - Exception: {ex.Message} | {ex.StackTrace} ");
                              
            }

            return _oResponse;
        }

        [HttpPost]
        [Route("GetServicesByRole")]
        public Response GetServicesByRole(ServiceByRoleDTO serviceByRoleDTO)
        {
            try
            {
                DateTime dtime = DateTime.Parse(serviceByRoleDTO.strDateTime);
                serviceByRoleDTO.strDateTime = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                Read read = new Read();
                List<Service> lstService = read.GetServicesByRole(serviceByRoleDTO);
                _oResponse.Values = lstService;
            }
            catch(Exception ex)
            {
                _oResponse = new Response(ExceptionCode.Exception, false);

                ArgumentException aex = new ArgumentException(ex.Message.ToString());
                _logger.LogCritical(aex, $"* GetServicesByRole - Exception: {ex.Message} | {ex.StackTrace} ");
            }

            return _oResponse;
        }
         
        [HttpPost]
        [Route("GetUserByUserName")]
        public Response GetUserByUserName(UserName oUserName)
        { 
            try
            {
                Read oRead = new Read();
                User oUser = oRead.GetUserByUserName(oUserName.strUserName);
                _oResponse.Values = oUser;
            }
            catch (Exception ex)
            {
                _oResponse = new Response(ExceptionCode.Exception, false);

                ArgumentException aex = new ArgumentException(ex.Message.ToString());
                _logger.LogCritical(aex, $"* GetUserByUserName - Exception: {ex.Message} | {ex.StackTrace} ");
            }      

            return _oResponse;
        }
    }
}
