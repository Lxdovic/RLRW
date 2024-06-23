namespace RLReplayWatcher.data;

internal sealed class ClientLoadoutOnline : RLRPClientLoadoutOnline {
    public new List<List<ProductAttribute>>? ProductAttributeLists { get; set; }

    public ClientLoadoutOnline Clone() {
        var productAttributeLists = new List<List<ProductAttribute>>();

        if (ProductAttributeLists != null)
            foreach (var productAttributeList in ProductAttributeLists.ToList()) {
                var newProductAttributeList = new List<ProductAttribute>();

                foreach (var productAttribute in productAttributeList)
                    newProductAttributeList.Add(productAttribute.Clone());

                ProductAttributeLists.Add(newProductAttributeList);
            }

        return new ClientLoadoutOnline {
            ProductAttributeLists = productAttributeLists
        };
    }
}