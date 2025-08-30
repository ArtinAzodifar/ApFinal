# AP Final Project: 2D Co-op Platformer

A cooperative **2D platformer game** built in Unity, featuring **split-screen**, **online multiplayer**, puzzles, enemies, and an epic boss fight.

---

## 🎮 Preview 
<img width="1747" height="952" alt="image" src="https://github.com/user-attachments/assets/64d88a3d-ef90-4e15-87b8-42141e8a0c51" />

<img width="1433" height="722" alt="image" src="https://github.com/user-attachments/assets/5b0fd59b-2708-4d9e-8db4-aff692bdb02e" />

<img width="1742" height="975" alt="image" src="https://github.com/user-attachments/assets/4d36b7c6-84b2-4055-9be9-31cba8c6f7ef" />

<img width="1810" height="986" alt="image" src="https://github.com/user-attachments/assets/54d82231-549a-421a-b2a1-ee7177b941a8" />




---

## ✨ Features  
- **Local & Online Co-op** (PlayFab login + LAN connection)  
- **Two unique characters**:  
  - ⚔️ Melee swordsman  
  - 🏹 Ranged archer  
- **Melee & ranged enemies**  
- **Puzzles & map challenges**  
- **Save/Load system** using JSON  
- **Boss fight** at the end of Level 3 

---

## 🕹️ Game Modes  

### Local Mode  
- Start a **New Game** or **Load a Saved Game**  

### Online Mode  
<img width="1738" height="967" alt="image" src="https://github.com/user-attachments/assets/5ba81bc4-6a86-4e44-954c-df3443dae241" />

1. Login via **PlayFab**  
2. Choose **I am Host** or **I am Client**  
   - Host creates lobby  
   - Client enters host IP  (you need host's ip in the wifi network)
   - Both Host & Client should be in the same Wifi network
3. Enter **Lobby** → Select character (swordsman/archer)  
4. Press **Start** to begin 

---

## 🎯 Gameplay Rules  
- Levels 1 & 2: Game Over if **any player dies**  
- Level 3: Game Over only if **both players die**  
- Win: Defeat the **final boss** 

---

## ⚙️ Technical Details  
- Engine: **Unity 2D**  
- Programming language: **C#**
- Networking: **Unity Netcode for GameObjects**  
- Authentication: **PlayFab**  
- Save System: **JSON**  
- Scenes:  
  - **Level 1 & 2** → Side view split-screen  
  - **Level 3** → Shared top-down camera
 

## Credits

    Developed by ArtinAzodifar & MehdiAfshari
