# tezos.blue wallet

Native Tezos wallet on Android, iOS and Windows

The wallet built from this code can be downloaded via

http://tezos.blue

## About
The tezos.blue wallet is our flagship example of what can be built on the tezos.blue cross-platform development system for Tezos apps.
It offers all basic features like
- import and activation of fundraiser wallets
- delegation
- creation of new accounts
- transfers
- backup and restore of identities

All of this with 
- the enhanced security of the tezos.blue [client engine](https://github.com/tezos-blue/client)
- real-time updates from the network for a truly live user experience
- minimum resource usage and network traffic

## Architecture and development environment

The whole tezos.blue system is written in C#.
About 98% of the client code is located in .NET Standard 2.0 assemblies, making it fit to run on any major platform.

The UI is built on [Xamarin](https://visualstudio.microsoft.com/xamarin/).
This opens a build pipeline towards fully native apps on Android, iOS and Windows, ready for the app stores.

The general architecture follows the well-known MVVM pattern with the associations
1. Model = [Client Engine](https://github.com/tezos-blue/client)
2. ViewModel = Application assembly
3. View = Xamarin.Common assembly


