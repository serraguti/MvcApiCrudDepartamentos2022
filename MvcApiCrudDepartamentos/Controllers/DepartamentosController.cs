using Microsoft.AspNetCore.Mvc;
using MvcApiCrudDepartamentos.Models;
using MvcApiCrudDepartamentos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApiCrudDepartamentos.Controllers
{
    public class DepartamentosController : Controller
    {
        private ServiceDepartamentos service;
        public DepartamentosController(ServiceDepartamentos service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.service.GetDepartamentosAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await this.service.FindDepartamentoAsync(id));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await this.service.FindDepartamentoAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Departamento dept)
        {
            await this.service.UpdateDepartamentoAsync(dept.IdDepartamento
                , dept.Nombre, dept.Localidad);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departamento dept)
        {
            await this.service.InsertDepartamentoAsync(dept.IdDepartamento
                , dept.Nombre, dept.Localidad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.service.DeleteDepartamentoAsync(id);
            return RedirectToAction("Index");
        }
    }
}
