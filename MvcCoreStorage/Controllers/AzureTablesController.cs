﻿using Microsoft.AspNetCore.Mvc;
using MvcCoreStorage.Models;
using MvcCoreStorage.Services;

namespace MvcCoreStorage.Controllers
{
    public class AzureTablesController : Controller
    {
        private ServiceStorageTables service;



        public AzureTablesController(ServiceStorageTables service)
        {
            this.service = service;
        }



        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes =
            await this.service.GetClientesAsync();
            return View(clientes);
        }



        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this.service.CreateClientAsync
            (cliente.IdCliente, cliente.Nombre,
            cliente.Salario, cliente.Edad, cliente.Empresa);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete
        (string partitionKey, string rowKey)
        {
            await this.service.DeleteClienteAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }



        public IActionResult ClientesEmpresa()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ClientesEmpresa(string empresa)
        {
            List<Cliente> clientes =
            await this.service.getClientesEmpresaAsync(empresa);
            return View(clientes);
        }

        public async Task<IActionResult> Details(string partitionkey,string rowkey) 
        {
            Cliente cliente = await
            this.service.FindClienteAsync(partitionkey, rowkey);
            return View(cliente);

        }
    }
}

