[![messaging-app-logo](Project2/Project2/Content/Images/CondorChatLogo2.png)](https://condor-chat.azurewebsites.net)

# The Project 
Condor Chat is a web based real time chat application. The App has two main functionalities: Private and Public Chat.

  - In the Private Chat, the user selects the particular user he/she wants to chat with. If the recipient is online, he/she receives the messages in real time. Otherwise, the messages are loaded once the recipient signs in.

  - Public Chat is essentially a chat room. Users can send messages to more than one recipients. All messages are stored in the database and can be loaded via the load history button in order for users to catch up with the conversation.

All messages, private and public, are stored in the database.

There are three user types:
  - _**Simple User**_: This type of user can send and receive private and public messages, edit his/her profile, see his/her stats and download his/her messages in a ".txt" file.
  - _**Admin**_: This type of user has all the _Simple User_ functionalities. Moreover, the _Admin_ user type has CRUD privileges over the users.
  - _**God**_: This type of user has all the _Simple User_ functionalities. Moreover, the _God_ user type can view all the messages exchanged by the users.

## Installation
For local use the following steps are required:
  - Installation of Microsoft Visual Studio
  - On Microsoft Visual Studio, at the tab _Tools_, select _Package Manager Console_ and type the following commands:
    - `add-migration "initial"` (or any other name that you prefer)
    - `update-database`
  - Enjoy the App

## How to Login
Sign up as a new user or login to your existing account.

For user type 'God' privileges please sign in using the following credentials: Username: _**Goddess**_, Password: _**12qw!@Qw**_.

For user type 'Admin' privileges please sign in using the following credentials: Username: _**Admin**_, Password: _**12qw!@Qw**_.

## Website Url
https://condor-chat.azurewebsites.net

## Technologies Used
### Front-end
  - JavaScript
  - jQuery
  - HTML (HTML 5)
  - CSS (CSS 3)
  - Bootstrap
  - ASP .NET MVC 5
  - Moment.js
  - Notify.js

### Back-end
  - C#
  - SignalR
  - Entity Framework
  - LINQ
  - SQL Server

## Team Condors
 - Gerogiannis Giannis - [giannisgeros](https://github.com/giannisgeros)
 - Kanellopoulos Nikolas - [nickanel](https://github.com/nickanel)
 - Karipidis Anestis - [akaripidis](https://github.com/akaripidis)
 - Koronaios Theodore - [theodoreKor](https://github.com/theodoreKor)
 - Stavrianakis Nikos - [NiStav](https://github.com/NiStav)

## Requirements
See [REQUIREMENTS.md](REQUIREMENTS.md) for more information.

## License
Distributed under the MIT license. See [LICENSE.md](LICENSE.md) for more information.