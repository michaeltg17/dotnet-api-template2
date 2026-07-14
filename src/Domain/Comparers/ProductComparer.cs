using Domain.Models;
using System.Collections.Generic;

namespace Domain.Comparers;

public class ProductComparer : IEqualityComparer<Product>
{
    public bool Equals(Product? x, Product? y) => x is { } px && y is { } py && px.Id == py.Id;
    public int GetHashCode(Product obj) => obj.Id.GetHashCode();
}