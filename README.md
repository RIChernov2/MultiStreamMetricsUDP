# Переключение языка / Language Switch

- [Russian](#russian)
- [English](#english)

---

## <a name="russian"></a> Задача на русском / Task in Russian.

**Условие:** создать решение, которое максимально быстро считает статистические параметры по котировкам с "биржи". 
Для реализации необходимо создать два консольных приложения:

* **1-е приложение-сервер:** бесконечно генерирует случайные числа в диапазоне (для эмуляции предметной области - потока котировок
с биржи), рассылает по udp multicast, без задержек. Диапазон и мультикаст-группа настраивается через отдельный хмл-конфиг.

* **2-е приложение-клиент:** Принимает данные по udp multicast, считает по всем полученным: среднее арифметическое,
стандартное отклонение, моду и медиану. Общее количество полученных котировок может быть от триллиона и выше.
Посчитанные значения выводит в консоль по требованию (нажатие энтер). Приложение должно контролировать
получение всех котировок, количество потерянных котировок (те которые не дошли до клиента по какой либо причине:
проблемы сети, клиент не успел вычитать и т.д.) должно выводиться совместно со статистикой Прием пакетов и расчеты
статистических параметров реализовать в разных потоках с минимальными задержками. Мультикаст-группа приема
должна настраиваться через отдельный хмл-конфиг (не в app.config). 

***Важное требование:***

Приложение должно быть максимально оптимизировано по скорости работы с учетом объема полученных данных и выдавать решение
как можно быстрее (в течении миллисекунды) - для бирж значение имеет каждая микросекунда. 
Приложение должно работать продолжительное время (неделя-месяц) без падений по внутренним ошибками,
а также в случае ошибок в сети.

---

## <a name="english"></a> Task in English / Задача на английском.

**Condition:** Create a solution that calculates statistical parameters for market quotes as quickly as possible.

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

