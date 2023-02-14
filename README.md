# DevHoldableEngine
DevHoldableEngine is a physics based holdable system for Gorilla Tag.

## Item Logic
Grabbing and releasing items is **very easy**. <br />

- Go to the item you want to pick up, put one of your hands over that item, and then hold down grip. When doing so you will hold the item, but when you release grip, it will also release the item you're holding. <br />
- The item acts like an ingame item too, for instance if you have a custom item in your right hand, you're not able to pick up any others with that hand until you release that custom item. <br />

## Item Customization
You can customize a ton of information relating to custom items, grab distance, throw force, etc. <br />
Here's a table of some of them: <br />
| Variable Name  | Action                                                              |
| -------------- | ------------------------------------------------------------------- |
| PickUp         | If we can pick up the item                                          |
| Rigidbody      | The Rigidbody used for the physics                                  |
| Distance       | The distance required for you to grab the item                      |
| ThrowForce     | How much force is added onto the physics once you release the item  |

## Disclaimer
This product is not affiliated with Gorilla Tag or Another Axiom LLC and is not endorsed or otherwise sponsored by Another Axiom LLC. Portions of the materials contained herein are property of Another Axiom LLC. © 2021 Another Axiom LLC.
