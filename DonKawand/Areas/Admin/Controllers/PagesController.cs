﻿using DonKawand.Models.Data;
using DonKawand.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DonKawand.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of PageViewModel
            List<PageVM> pagesList;


            using (Db db = new Db())
            {
                //Initialize the list                 //Lambda expression
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }


            //Return view with list
            return View(pagesList);
        }

        //GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        //POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Declare Slug
                string slug;

                //Initialize pageDTO
                PageDTO dto = new PageDTO();

                //DTO Title
                dto.Title = model.Title;

                //Check for amd set s;ig of need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or slug already exist");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Set TempData message
            TempData["SM"] = "You hve added a new page";

            //Redirect
            return RedirectToAction("AddPage");
        }

        //GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {

            //Declare PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist");
                }

                //Initialize PageVM
                model = new PageVM(dto);
            }
            

            //Return view with the model
            return View(model);
        }

        //POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //Check model state
            if(! ModelState.IsValid)
            {

                return View(model);
            }

            using (Db db = new Db())
            {
           
                //Get page id
                int id = model.Id;

                //Initialize the slug
                string slug = "home";

                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //DTO the title
                dto.Title = model.Title;

                //Check for slug and set it if need be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug) )
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //Make sure the title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || 
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or slug already exist");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
   
                //Save the DTO
                db.SaveChanges();
            }


            //Set TempData message
            TempData["SM"] = "You hve added a new page";


            //Redirect the page

            return RedirectToAction("EditPage");
        } //END OF edit page GET

        //GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //Declare PageVM
            PageVM model;


            using (Db db = new Db())
            {
                  
                //Get the page
                PageDTO dto = db.Pages.Find(id); 

                //Confirm page exists
                if(dto== null)
                {
                    return Content("The page does not exist.");
                }

                //Intialize Page VM
                model  = new PageVM(dto);

            }
         

            //Return view with the model

            return View(model);
        } //END OF Page Details GET

        //GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using(Db db = new Db()){
            //Get the page
            PageDTO dto = db.Pages.Find(id);

            //Remove the page
            db.Pages.Remove(dto);

            //Save 
            db.SaveChanges();
            }

            //Redirect
            return RedirectToAction("Index");
        }//END OF DELETE PAGE

        //POST: Admin/Pages/ReorderPages/id
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            

            using (Db db = new Db())
            {
                //Set initial count
                int count = 1;

                //Declare PageDTO
                PageDTO dto;

                //Set sorting for each page
                foreach(var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count ++;
                }
            }
        }//END OF ReorderPages


        //GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {

            //Declare the model
            SidebarVM model;

            using (Db db = new Db())
            {
               
                //Get the DTO and searching for the sidebar data transfer object
                SidebarDTO dto = db.Sidebar.Find(1);

                //Initialize the model
                model = new SidebarVM(dto);
            }


            //Return view with model
            return View(model);
        }


        //POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {

            using (Db db = new Db())
            {
                //Get the DTO and searching for the sidebar data transfer object
                SidebarDTO dto = db.Sidebar.Find(1);

                //DTO the body
                dto.Body = model.Body;

                //Save the changes
                db.SaveChanges();        
            }

            //Set TempData message
            TempData["SM"] = "You have edited the sidebar!";

            //Redirect
            return RedirectToAction("EditSidebar");
        }




    }//END OF CLASS

}//END NAMESPACE