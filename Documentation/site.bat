"E:\OpenSSL\bin\openssl" genrsa -out %2.key 4096
"E:\OpenSSL\bin\openssl" req -new -key %2.key -out %2.csr
"E:\OpenSSL\bin\openssl" x509 -req -in %2.csr -CA %1.crt -CAkey %1.key -CAcreateserial -out %2.crt -days 3650
"E:\OpenSSL\bin\openssl" pkcs12 -export -in %2.crt -inkey %2.key -out %2.pfx