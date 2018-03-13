﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Model.Extensions;
using SimpleToDo.Service.Contracts;

namespace SimpleToDo.Web.Controllers
{
    public class ListController : Controller
    {
        private readonly IToDoListService _toDoListService;

        public ListController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        // GET: List
        public async Task<IActionResult> Index()
        {
            return View(await _toDoListService.GetToDoLists());
        }

        // GET: List/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _toDoListService.GetToDoListById(id.Value);


            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // GET: List/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: List/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListId,Name")] List list)
        {
            if (ModelState.IsValid)
            {
                await _toDoListService.CreateToDoList(list);

                this.AddAlertSuccess($"{list.Name} created successfully.");

                return RedirectToAction(nameof(Index));
            }

            return View(list);
        }

        // GET: List/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _toDoListService.GetToDoListById(id.Value);
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        // POST: List/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListId,Name")] List list)
        {
            if (id != list.ListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _toDoListService.UpdateToDoList(list);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool todoExists = await _toDoListService.ToDoListExists(id);

                    if (!todoExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                this.AddAlertSuccess($"{list.Name} updated successfully.");
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }

        // GET: List/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _toDoListService.GetToDoListById(id.Value);

            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // POST: List/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string listName = await _toDoListService.RemoveToDoList(id);

            this.AddAlertSuccess($"{listName} removed successfully.");

            return RedirectToAction(nameof(Index));
        }
    }
}
