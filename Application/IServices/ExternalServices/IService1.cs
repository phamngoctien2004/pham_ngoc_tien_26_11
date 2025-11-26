using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.ExternalServices
{
	public interface IService1
	{
		Task<List<Product>> GetAllProducts();
	}
}
