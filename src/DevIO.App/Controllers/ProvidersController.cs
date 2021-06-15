using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class ProvidersController : BaseController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;
        public ProvidersController(IProviderRepository providerRepository, IProviderService providerService, INotifier notifier ,IMapper mapper) : base (notifier)
        {
            _providerRepository = providerRepository;
            _providerService = providerService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAllAsync()));
        }

        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var providerViewModel = await GetAddressProvider(id);

            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        [ClaimsAuthorize("Providers", "Add")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Providers", "Add")]
        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid) return View(providerViewModel);
            
            var provider = _mapper.Map<Provider>(providerViewModel);
            
            await _providerService.AddAsync(provider);

            if (!ValidOperation()) return View(providerViewModel);
            
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Providers", "Edit")]
        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var providerViewModel = await GetAddressProductsProvider(id);
            
            if (providerViewModel == null)
            {
                return NotFound();
            }
            
            return View(providerViewModel);
        }

        [ClaimsAuthorize("Providers", "Edit")]
        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(providerViewModel);
            
            var provider = _mapper.Map<Provider>(providerViewModel);    

            await _providerService.UpdateAsync(provider);

            if (!ValidOperation()) return View(await GetAddressProductsProvider(id));

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Providers", "Delete")]
        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var providerViewModel = await GetAddressProvider(id);

            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        [ClaimsAuthorize("Providers", "Delete")]
        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var providerViewModel = await GetAddressProvider(id);

            if (providerViewModel == null) return NotFound();

            await _providerService.RemoveAsync(id);

            if (!ValidOperation()) return View(providerViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Providers", "Edit")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> AddressUpdateAsync(Guid id)
        {
            var provider = await GetAddressProvider(id);

            if (provider == null)
            {
                return NotFound();
            }

            return PartialView("_AddressUpdate", new ProviderViewModel { Address = provider.Address });
        }

        [ClaimsAuthorize("Providers", "Edit")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AddressUpdateAsync(ProviderViewModel providerViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if (!ModelState.IsValid) return PartialView("_AddressUpdate", providerViewModel);

            await _providerService.AddressUpdateAsync(_mapper.Map<Address>(providerViewModel.Address));

            if (!ValidOperation()) return PartialView("_AddressUpdate", providerViewModel);

            var url = Url.Action("GetAddress", "Providers", new { id = providerViewModel.Address.ProviderId });
            return Json(new {success = true, url });
        }

        [AllowAnonymous]
        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var provider = await GetAddressProvider(id);

            if (provider == null)
            {
                return NotFound();
            }

            return PartialView("_AddressDetails", provider);
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
