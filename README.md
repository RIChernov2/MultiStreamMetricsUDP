([🇺🇸 Read in English](README.md) | [🇷🇺 Читать на русском](README.ru.md))

# Test Assignment

---

**Condition:** Create a solution that calculates statistical parameters for market quotes
as quickly as possible.

To implement this, two console applications need to be created:

* **1st Application - Server:** It continuously generates random numbers within a specified range
(to simulate a stream of market quotes). It broadcasts these numbers via UDP multicast, without
delays. The range and multicast group should be configured through a separate XML config file.

* **2nd Application - Client:** It receives data via UDP multicast and calculates the following
statistical parameters for all received data: arithmetic mean, standard deviation, mode, and
median. The total number of received quotes can be from a trillion or more. The calculated
values should be displayed in the console upon request (pressing Enter). The application
should track the total number of received quotes and display the number of lost quotes
(those that did not reach the client due to network issues, the client not being fast enough
to process, etc.). The reception of packets and the calculation of statistical parameters
should be implemented in separate threads to minimize delays. The multicast group for
receiving data should be configured through a separate XML config file (not in app.config).

***Key requirements:***

The application must be highly optimized for speed, considering the volume of data, and must provide results as quickly as possible (within milliseconds) — every microsecond counts for the stock exchange.
The application should run continuously for extended periods (a week to a month) without crashing due to internal errors, and should be resilient to network errors.

---
