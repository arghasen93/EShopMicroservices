using Marten.Schema;

namespace Catalog.API.Data;

public class CataloginitialData : IInitialData
{
    private static IEnumerable<Product> Objects =>
    [
        new Product
        {
            Name = "Sample Product 1",
            Description = "This is a sample product description.",
            ImageFile = "product1.jpg",
            Price = 19.99m,
            Categories = ["Category1", "Category2"]
        },
        new Product
        {
            Name = "Sample Product 2",
            Description = "This is another sample product description.",
            ImageFile = "product2.jpg",
            Price = 29.99m,
            Categories = ["Category3"]
        }
    ];

    public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
    {
        using var session = store.LightweightSession();

        if(await session.Query<Product>().AnyAsync(cancellationToken))
            return;

        session.Store(Objects);
        await session.SaveChangesAsync(cancellationToken);
    }
}
