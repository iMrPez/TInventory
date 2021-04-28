![](https://img.shields.io/static/v1?label=Version&message=1.0.0&color=%3CCOLOR%3E) ![](https://img.shields.io/static/v1?label=Unity&message=2020.2.2f1&color=%3CCOLOR%3E)

## TInventory

TInventory is a size-based inventory framework for Unity. The framework makes setting up a size-based inventory for your game quick and easy. 

#### Features

- Non Square Inventories
- Size-based Items
- Context Menu
- Item Actions
- Item Variants
- Item Containers
- Action Slots
- Windows



#### Tutorials



##### Initialize TInventory

TInventory has a startup wizard that makes getting TInventory setup in your scene very easy. The first step is to open the "Startup Wizard" menu.

![](https://i.imgur.com/q3e5spv.png)

Then finally click "Initialize TInventory".

![](https://i.imgur.com/FBmXnTg.png)

These 3 new GameObjects will be added, though you may already have "Canvas" and "EventSystem" in your scene. If you do, they will not be added.

![](https://i.imgur.com/FglXNGN.png)

The "Window Canvas" field is the canvas the windows will be instantiated to, so if you don't want the window here, make sure to switch this.

![](https://i.imgur.com/Z0xFLmT.png)





##### Create Window

There are a couple ways to create windows, the first being simply dropping the window prefab inside the Canvas.

![](https://i.imgur.com/OlwFhLx.png)

The second way is to create the window through code, this can be done by calling the CreateNewWindow method inside of the Inventory class.

![](https://i.imgur.com/zpaAd7Y.png)

Creating a window with the CreateNewWindow method, requires a title, and a Vector2 size for the window's size.





##### Create New Container

The first step to creating a new container is to make a Container Scriptable Object, this can be done by right clicking in your file manager and going to "Create/TInventory/Container".

![](https://i.imgur.com/6kNb3WI.png)

Here is the Container Data inspector. The container size inside the <span style="color:green">GREEN</span> box allows you to change the size of matrix at the bottom of the inspector in the <span style="color:red">RED</span> square. The matrix at the bottom allows you to set container groups, you do this by specifying groups through numbers, for example a square with a group id of 5 will result in a 2x2 box. If a 0 is placed, nothing will be displayed for that box, if a 1 is placed, a solo slot will be displayed. A group has to be rectangular, and only one group with the same id may be used per container besides 1s.

Container Name - Name of the container.

Filter - Allows you to add a filter which block items that don't match said type from being placed inside it.

Container Size - Changes the size of the container's editable slots.

Group - Lets you set all the slots to a single group.

Container Data Matrix - A matrix style representation of the container groups that will be created at runtime. EXAMPLE shows in game vs matrix.

EXAMPLE

![](https://i.imgur.com/7fCSi0Y.png)

![](https://i.imgur.com/QLLOlqT.png)

In order for containers to be saved with item containers, or have the container data be grabbed in code, the container needs to be added to the Inventory.

![](https://i.imgur.com/FZGnF2L.png)



The container can now be added through code to our previously created window, OR you can just make a field for the Container Data and not have the use the singleton Inventory method in order to get the Container Data which I personally would recommend.

![](https://i.imgur.com/Emj9eNM.png)





##### Create New Item / Add Item

Creating works in a similar way to creating containers, the first step is to create a new Scriptable Object by right clicking the file manager and going to "Create/TInventory/Item/Item Data".

![](https://i.imgur.com/J7vNmFx.png)



The Item Data inspector lets you determine the characteristics of the item. 

Name - Name of the item.

Short Name - The name that would be displayed if the full name doesn't fit.

Id - The items id, this should be unique to the item.

Item Type dropdown - Determines the type of item it is.

Max Image Size - The max size the image can be stretched to.

Item Slot Size - The amount of slots the item takes up.

Max Item Count - Max amount the item can be stacked to, 1 = Not stackable.

Description - Description of the item, not currently displayed anywhere.

![](https://i.imgur.com/idGbRPA.png)



EXAMPLE

![](D:\TInventory\HowToCreateItem\3.png)



The item now needs to be added to the ItemFactory.

![](D:\TInventory\HowToCreateItem\4.png)



The item can now be added to our container we made above.

![](D:\TInventory\HowToCreateItem\5.png)



#### Roadmap

Trello | https://trello.com/b/xuL95njz/tinventory

