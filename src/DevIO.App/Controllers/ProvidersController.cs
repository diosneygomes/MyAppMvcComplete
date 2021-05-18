﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;

namespace DevIO.App.Controllers
{
    public class ProvidersController : BaseController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        public ProvidersController(IProviderRepository providerRepository, IMapper mapper)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAllAsync()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var providerViewModel = await GetAddressProvider(id);

            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid) return View(providerViewModel);
            
            var provider = _mapper.Map<Provider>(providerViewModel);
            
            await _providerRepository.AddAsync(provider);
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var providerViewModel = await GetAddressProductsProvider(id);
            
            if (providerViewModel == null)
            {
                return NotFound();
            }
            
            return View(providerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(providerViewModel);
            
            var provider = _mapper.Map<Provider>(providerViewModel);

            await _providerRepository.UpdateAsync(provider);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var providerViewModel = await GetAddressProvider(id);

            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var providerViewModel = await GetAddressProvider(id);

            if (providerViewModel == null) return NotFound();

            await _providerRepository.RemoveAsync(id);

            return RedirectToAction("Index");
        }

        private async Task<ProviderViewModel> GetAddressProvider(Guid id)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetAddressProviderAsync(id));
        }

        private async Task<ProviderViewModel> GetAddressProductsProvider(Guid id)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProductsAddressProviderAsync(id));
        }
    }
}