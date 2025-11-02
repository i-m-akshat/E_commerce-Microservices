using ProductAPI.Domain.Entities;

namespace ProductAPI.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static Product ToProduct(this ProductDTO dto)
        {
            Product product = new Product()
            {
                Id=dto.id,
                Name=dto.name,
                Quantity=dto.Quantity,
                Price=dto.Price,
                Description=dto.description
            };
            return product;
        }
        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            //return single 

            if(product is not null ||products is null)
            {
                var singleProduct = new ProductDTO
                (
                   product!.Id,
                   product!.Name!,
                   product!.Description!,
                   product!.Quantity,
                   product!.Price

                );
                return (singleProduct,null);
            }
            //return list 
           else if (product is null || product is not null)
            {
                var _Products = products!.Select(x => new ProductDTO(
                   x!.Id,
                   x!.Name!,
                   x!.Description!,
                   x!.Quantity,
                   x!.Price)).ToList();
                return (null, _Products);
            }
            else
            {
                return (null, null);
            }
        }
    }
}
