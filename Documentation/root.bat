"E:\OpenSSL\bin\openssl" genrsa -des3 -out %1.key 4096
"E:\OpenSSL\bin\openssl" req -x509 -new -key %1.key -days 10000 -out %1.crt