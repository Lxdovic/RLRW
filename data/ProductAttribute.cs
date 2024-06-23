namespace RLReplayWatcher.data;

internal sealed class ProductAttribute : RLRPProductAttribute {
    private const int MAX_VALUE = 14;
    public new bool Unknown1 { get; set; }
    public new uint ClassIndex { get; set; }
    public new string? ClassName { get; set; }
    public new bool HasValue { get; set; }
    public new object? Value { get; set; }

    public ProductAttribute Clone() {
        return new ProductAttribute {
            Unknown1 = Unknown1,
            ClassIndex = ClassIndex,
            ClassName = ClassName,
            HasValue = HasValue,
            Value = Value
        };
    }
}