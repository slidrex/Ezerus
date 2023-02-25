using UnityEngine;

namespace Ezerus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory Item/Material")]
    public class Material : StackableItem
    {
        public override Type ItemType => Type.Material;
        protected override void OnItemSelect(Entity entity)
        {
            Debug.Log("Material was selected!");
        }
        protected override void OnItemPrimaryUse(Entity entity)
        {
            Debug.Log("Material was primary used!");
        }
        protected override void OnItemDeselect(Entity entity)
        {
            Debug.Log("Material was deselected!");
        }
        protected override void OnItemSecondaryUse(Entity entity)
        {
            Debug.Log("Material was secondary used!");
        }
    }
}
