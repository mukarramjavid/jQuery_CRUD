using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessOperation.Interfaces;
using demo_master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserLive.Models;

namespace UserLive.Controllers
{
    public class NewUserController : Controller
    {
        private readonly IBOUsers _IBOUsers;
        public NewUserController(IBOUsers IBOUsers)
        {
            _IBOUsers = IBOUsers;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult InsertMaster()
        {

            return View();
        }
        [HttpPost]
        public ActionResult InsertMasterData(NewUserVM data)
        {
            var dataResult = JsonConvert.DeserializeObject<List<NewUserVM>>(data.dataList);

            foreach (var item in dataResult)
            {
                var user = new UserModel
                {
                    userEmail = item._userEmail,
                    userName = item._userName,
                    userPhone = item._userPhone,
                    userPwd = item._userPwd
                };
                _IBOUsers.InsertMaster(user);
            }


            return RedirectToAction("InsertMaster");
        }


        public JsonResult GetMasterUsersLists()
        {
            //var sa = new JsonSerializerSettings();
            var list = _IBOUsers.GetMasterUsers();
            List<NewUserVM> objList = new List<NewUserVM>();
            foreach (var item in list)
            {
                NewUserVM uvm = new NewUserVM();
                uvm._userName = item.userName;
                uvm._userEmail = item.userEmail;
                uvm._userPhone = item.userPhone;
                uvm._userPwd = item.userPwd;
                uvm._userId = item.userId;

                objList.Add(uvm);


            }
            var obj = objList;
            return Json(obj, new JsonSerializerSettings());
        }

        public JsonResult DeleteRecordbyID(int id)
        {
            _IBOUsers.DeleteMasterRecord(id);
            return Json(new JsonSerializerSettings());
        }


        public JsonResult GetMasterbyId(int id)
        {
            var rcrd = _IBOUsers.GetMasterById(id);
            var uvm = new NewUserVM()
            {
                _userId = rcrd.userId,
                _userName = rcrd.userName,
                _userEmail = rcrd.userEmail,
                _userPwd = rcrd.userPwd,
                _userPhone = rcrd.userPhone

            };
            return Json(uvm, new JsonSerializerSettings());
        }

        [HttpPost]
        public JsonResult GetMasterbyId(int id, NewUserVM uvm)
        {
            var user = new UserModel()
            {
                userId = id,
                userEmail = uvm._userEmail,
                userName = uvm._userName,
                userPhone = uvm._userPhone,
                userPwd = uvm._userPwd
            };
            int rcrd = _IBOUsers.UpdateMasterUser(id, user);

            return Json(new JsonSerializerSettings());
        }
    }
}

    /*********************************************************New User MODEL***************************************************************/
   public class NewUserVM
    {
        public int _userId { set; get; }
        public string _userName { set; get; }
        public string _userEmail { set; get; }
        public string _userPwd { set; get; }
        public string _userPhone { set; get; }

        public string dataList { set; get; }

    }

    /*********************************************************User MODEL***************************************************************/
public class UserModel
    {
        public int userId { set; get; }
        public string userName { set; get; }
        public string userEmail { set; get; }
        public string userPwd { set; get; }
        public string userPhone { set; get; }
    }