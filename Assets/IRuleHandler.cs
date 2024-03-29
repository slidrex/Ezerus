public interface IRuleHandler
{
    public enum Rule
    {
        BlockCamera,
        BlockMovement,
        BlockInteraction,
        BlockAbilities,
        BlockPlayerSprint
    }
    public enum ChangeType
    {
        Add,
        Remove
    }
    public System.Collections.Generic.List<Rule> Rules { get; }
    public void AddRule(Rule rule);
    public void RemoveRule(Rule rule);
    public bool ContainsRule(Rule rule);
    public System.Action<Rule, ChangeType> RuleChangeCallback { get; set; }
}
