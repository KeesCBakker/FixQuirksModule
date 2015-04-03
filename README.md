# FixQuirksModule
Asp.Net module that fixes the quirks mode by forcing IE to the right version without using the edge header. This leaves forced debugging in a lower version in tact.

Quirks mode refers to a technique used by some web browsers for the sake of maintaining backward compatibility with web pages designed for older browsers, instead of strictly complying with W3C and IETF standards in standards mode.

## The problem
Sometimes the browser decides to go into quirks mode. The `X-UA-Compatible` header can be used to instruct IE to load a specific version. But what if the user is debugging and asks for IE9? The browser changes it query string, but will be forced back if we just add the `IE=edge`. That's why we created a module that inspects the Trident token and adds the right header.

## Usage
Compile the DLL and add the following to the web.config of your project:

```xml
<system.webServer>
  <modules>
		<remove name="FixQuirksMode" />
		<add name="FixQuirksMode" 
		     type="KeesTalksTech.WebUI.ExtendedModules.FixQuirksModule, KeesTalksTech.FixQuirks"/>
	</modules>
</system.webServer>
```

Original: http://keestalkstech.com/2014/12/fixing-quirks-mode-with-a-dll/




