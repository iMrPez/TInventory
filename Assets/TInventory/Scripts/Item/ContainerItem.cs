using System;
using System.Collections.Generic;
using TInventory.Container;
using TInventory.ContextMenu.Action;
using TInventory.Item.Action;
using UnityEngine;

namespace TInventory.Item
{
    public class ContainerItem : Item
    {

        [SerializeField] private ContainerData _containerData;

        private Window.Window _window;
        private Container.Container _container;

        public Container.Container Container => _container;
        public ContainerData ContainerData => _containerData;

        private string _containerJson = "";

        public override List<IItemAction> GetReleaseActions()
        {
            return new List<IItemAction>()
            {
                new PlaceAction(),
                new AttachAction(),
                new ContainerAction()
            };
        }

        public override List<IOption> GetContextMenuActions()
        {
            return new List<IOption>()
            {
                new OpenOption(this),
                new DeleteOption(this)
            };
        }

        public override IOption GetDoubleClickAction()
        {
            return new OpenOption(this);
        }

        /// <summary>
        /// Shows container
        /// </summary>
        public void Show()
        {
            InitContainer();
            
            InitWindow();
            
            // Add container content to window
            if (!(_container is null))
            {
                _window.AddContainer(_container);
            }
            
            _window.Show();
        }

        /// <summary>
        /// Check if item is a child of this item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if item is a child of this item</returns>
        public bool IsChildOf(Item item)
        {
            if (_container is null)
            {
                InitContainer();
            }

            foreach (var containedItem in _container.Items)
            {
                if (containedItem is ContainerItem containerItem)
                {
                    if (containedItem == item) return true;
                    
                    if (containerItem.IsChildOf(item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        
        /// <summary>
        /// Checks if container is a child of this container
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>True if the container is a child of this container</returns>
        public bool IsChildContainerOf(Container.Container container)
        {
            if (_container is null)
            {
                InitContainer();
            }

            foreach (var containedItem in _container.Items)
            {
                if (containedItem is ContainerItem containerItem)
                {
                    if (containerItem._container == container) return true;
                    
                    if (containerItem.IsChildContainerOf(container))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        
        /// <summary>
        /// Creates window if one isn't already shown
        /// </summary>
        private void InitWindow()
        {
            if (_window is null)
            {
                var size = _container.RectTransform.sizeDelta + new Vector2(20f, 140f);

                _window = Inventory.CreateNewWindow(_containerData.containerName, size);
                
                _window.WindowClosedHandler += () =>
                {
                    _containerJson = JsonUtility.ToJson(_container.GetModel());
                    _window = null;
                    _container = null;
                };
            }
        }

        
        /// <summary>
        /// Create Container if one isn't already made and loads the container
        /// </summary>
        private void InitContainer()
        {
            if (_container is null)
            {
                _container = Inventory.CreateNewContainer();
                _container.InitializeContainer(_containerData);

                if (_containerJson != "")
                {
                    _container.LoadModel(_containerJson);
                }
            }
        }

        public override object GetModel()
        {
            var itemModel = JsonUtility.ToJson(base.GetModel());

            var containerModel = _container is null ? _containerJson : JsonUtility.ToJson(_container.GetModel());

            var containerItemModel = new ContainerItemModel(itemModel, containerModel, Inventory.Instance.GetContainerDataId(_containerData));

            return new ItemModelWrapper(data.id, containerItemModel, ContainerGroup.id, SlotPosition);
        }

        public override bool LoadModel(string modelJson)
        {
            // Convert Json back to models
            var m = JsonUtility.FromJson<ContainerItemModel>(modelJson);
            var itemModel = JsonUtility.FromJson<ItemModelWrapper>(m.itemModelJson);

            // Load item data
            base.LoadModel(itemModel.model);
            _containerData = Inventory.Instance.GetContainerDataFromId(m.id);
            _containerJson = m.containerModelJson;
            return true;
        }

        public void AddItem(Item item, Vector2 slot, ContainerGroup containerGroup)
        {
            if (_container is null)
            {
                InitContainer();
            }
            
            _container.PlaceItemAt(slot, containerGroup, item);
        }

        public bool CanAddItem(Item item, out (Vector2 slot, ContainerGroup containerGroup) location)
        {
            if (_container is null)
            {
                InitContainer();
            }
            
            bool canAdd = _container.CanAddItem(item, out location);

            return canAdd;
        }
    }

    public struct ContainerItemModel
    {
        public string itemModelJson;
        public string containerModelJson;
        public int id;

        public ContainerItemModel(string itemModelJson, string containerModelJson, int id)
        {
            this.itemModelJson = itemModelJson;
            this.containerModelJson = containerModelJson;
            this.id = id;
            
        }
    }
}
