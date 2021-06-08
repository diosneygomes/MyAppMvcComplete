using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevIO.App.Data;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DevIO.App.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IProviderRepository providerRepository, IProductService productService, INotifier notifier ,IMapper mapper) : base (notifier)
        {
            _productRepository = productRepository;
            _providerRepository = providerRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsProvidersAsync()));
        }

        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var productViewModel = await FillProvidersAsync(new ProductViewModel());
            return View(productViewModel);
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await FillProvidersAsync(productViewModel);

            if (!ModelState.IsValid) return View(productViewModel);

            var imgPrefix = Guid.NewGuid() + "_";
            if (! await FileUploadAsync(productViewModel.ImageUpload, imgPrefix))
            {
                return View(productViewModel);
            }

            productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;              

            await _productService.AddAsync(_mapper.Map<Product>(productViewModel));

            if (!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            
            if (productViewModel == null) return NotFound();
            return View(productViewModel);
        }

        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            var productUpdate = await GetProduct(id);
            productViewModel.Provider = productUpdate.Provider;
            productViewModel.Image = productUpdate.Image;

            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";
                if (!await FileUploadAsync(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }
                productUpdate.Image = imgPrefix + productViewModel.ImageUpload.FileName;
            }

            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Price = productViewModel.Price;
            productUpdate.Active = productViewModel.Active;


            await _productService.UpdateAsync(_mapper.Map<Product>(productUpdate));

            if (!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProduct(id);
            
            if (product == null) return NotFound();

            return View(product);

        }

        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProduct(id);

            if (product == null) return NotFound();

            await _productService.RemoveAsync(id);

            if (!ValidOperation()) return View(product);

            TempData["Sucesso"] = "Produto excluído  com sucesso!";

            return RedirectToAction("Index");
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product =  _mapper.Map<ProductViewModel>(await _productRepository.GetProviderProductAsync(id));
            product.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAllAsync());
            return product;
        }

        private async Task<ProductViewModel> FillProvidersAsync(ProductViewModel productViewModel)
        {
            productViewModel.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAllAsync());
            return productViewModel;
        }

        private async Task<bool> FileUploadAsync(IFormFile file, string imgPrefix)
        {
            if (file.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty,"Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);        
            }
            return true;
        }
    }
}
