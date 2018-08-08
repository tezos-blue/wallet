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
- the [enhanced security](https://github.com/tezos-blue/client/blob/master/SecurityReview.md) of the tezos.blue [client engine](https://github.com/tezos-blue/client)
- real-time updates from the network for a truly live user experience
- minimum resource usage and network traffic

## Architecture and Environment

The whole tezos.blue system is written in C#. But you may write in any CLI-compliant language.

About 98% of the client code is located in .NET Standard 2.0 assemblies, making it compatible with any major platform.

The UI is built on [Xamarin](https://visualstudio.microsoft.com/xamarin/).
This opens a direct build pipeline towards fully native apps on Android, iOS and Windows, ready for the app stores.

The general architecture follows the well-established MVVM pattern with the associations
1. Model = [Client Engine](https://github.com/tezos-blue/client)
2. ViewModel = [Application](https://github.com/tezos-blue/wallet/tree/master/Common/Application)
3. View = [Xamarin.Common](https://github.com/tezos-blue/wallet/tree/master/Common/Xamarin)

All these layers are shared code between the platforms.

## Build instructions

You are fine with the community edition of [Visual Studio](https://visualstudio.microsoft.com/downloads/). Enable "Mobile Development" when installing.

Just open the solution and build.

For iOS you will need to connect to a Mac with the [tools installed](https://docs.microsoft.com/en-us/xamarin/ios/get-started/installation/windows/connecting-to-mac/).

## Getting started with the code

Apart from some platform-specific implementations of required interfaces, the main app code is the same for all platforms.
Platform-dependent code can be found in [this folder](https://github.com/tezos-blue/wallet/tree/master/Xamarin).

Entry point for the app is the [App.xaml.cs file](https://github.com/tezos-blue/wallet/blob/master/Common/Xamarin/App.xaml.cs) in Common/Xamarin.

The same assembly ([Xamarin.Common](https://github.com/tezos-blue/wallet/tree/master/Common/Xamarin)) contains the shared description of all visuals.

The [Application](https://github.com/tezos-blue/wallet/tree/master/Common/Application) assembly describes the user experience in an abstract fashion. It is ignorant of the UI technology used and facilitates automated testing of the app behavior.

[Resources](https://github.com/tezos-blue/wallet/tree/master/Common/Resources) contains the strings for localization into currently 10 languages.



