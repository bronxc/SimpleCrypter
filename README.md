##### SimpleCrypter
SimpleCrypter is a really basic encryption tool to protect .Net programs, This
is more like a Proof of concept than a real tool since the protection key is
stored in plain text into the stub/launcher program. (in fact it uses a really
basic encryption algorithm: RC4). Maybe it can be used to create a more sophisticated
encryption tool via changing the used algorithm, and hiding the decryption key.


###### Build :
Open the Visual Studio Project and press build, the program should be copied in /bin
at the end of the compiling process.

###### Usage :
```
SimpleCrypter.exe <executable-file> <rc4key> <output-file>
```

###### Example :
```
SimpleCrypter.exe myprogram.exe t0ps3cr3t almostProtected.exe
```
