using Application.DTOs.Request.Product;
using Application.DTOs.Response.Product;
using Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    [Mapper]
    public partial class IProductMapper
    {
        [MapperIgnoreTarget(nameof(Product.Id))]
        public partial Product AddToEntity(AddProductReq req);

        public partial Product UpdateToEntity(UpdateProductReq req);

        public partial ProductResponse ToResponse(Product product);

        public partial List<ProductResponse> ToResponseList(List<Product> products);
    }
}
