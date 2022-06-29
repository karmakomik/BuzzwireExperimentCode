import serial
import io
import socket

host = socket.gethostname()
port = 8089                   # The same port as used by the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect(("localhost", port))

ser = serial.Serial('COM6', 9600)
sio = io.TextIOWrapper(io.BufferedRWPair(ser, ser))



while True:
    cc = str(ser.read().decode("utf-8"))
    #val = int(cc[0])
    if cc == '1':
        msg = "M;1;;;BuzzWireHit;Respondent completed the check out task\r\n"
    else:
        msg = "M;1;;;NoHit;Respondent completed the check out task\r\n"

    #msg = "E;1;BuzzWire;1;;;;Buzzwire;" + cc + ";"
    #msg = "M;1;;;CheckOut;Respondent completed the check out task\r\n"
    s.sendall(bytes(msg,"utf-8"))
    #print(cc)