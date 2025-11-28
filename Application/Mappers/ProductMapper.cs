using Application.DTOs.Request.Product;
using Application.DTOs.Response.Category;
using Application.DTOs.Response.Product;
using AutoMapper;
using Core.Entities;
using Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<AddProductReq, Product>();
            CreateMap<UpdateProductReq, Product>();
            CreateMap<Product, ProductResponse>();
        }
    }
}
