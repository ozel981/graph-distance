W niniejszym folderze znajdują się katalogi i pliki stworzone w ramach projektu szybkiej ścieżki
z przedmiotu Teoria Algorytmów i Obliczeń w r. ak. 2021/2022. Opis struktury:
- katalog Examples - w nim znajdują się przykładowe macierze sąsiedztwa reprezentujące
grafy wejściowe,
- katalog EXE - zawiera pliki wykonywalne programu.

Korzystanie z programu
1. Po uruchomieniu program pyta o ścieżkę pliku z macierzami sąsiedztwa. Gdy użytkownik nie poda
ścieżki, zostają one wczytane przez standardowe wejście. Dla każdego grafu należy podać rozmiar
macierzy oraz macierz sąsiedztwa.
2. Po wczytaniu macierzy użytkownikowi zostaje przedstawiona lista dostępnych algorytmów obliczania
dystansu pomiędzy grafami. Użytkownik może wybrać dowolne z nich podając odpowiadające im litery.
3. Program wykonuje wybrane algorytmy na podanych wcześniej macierzach oraz wypisuje macierze po
przekształceniu, obliczony dystans oraz czas działania algorytmu.

Przykładowe uruchomienie
./GraphDistance.exe
Podaj ścieżkę do pliku z wejściem (jeśli grafy mają być wpisane ręcznie, wciśnij ENTER):
../../../../../examples/simple-graphs.txt

Lista algorytmów:
a) algorytm aproksymacyjny
k) algorytm aproksymacyjny z kolorowaniem
d) algorytm dokładny

Podaj litery oznaczające algorytmy oddzielone spacją: 
a k d

===== ALGORYTM APROKSYMACYJNY =====

Grafy G i H przed zmianą etykiet wierzchołków
Graf o liczbie 5 wierzchołków     Graf o liczbie 5 wierzchołków
0, 1, 1, 0, 0,                    0, 0, 0, 0, 1, 
1, 0, 0, 1, 0,                    0, 0, 0, 0, 1, 
1, 0, 0, 1, 0,                    0, 0, 0, 1, 0, 
0, 1, 1, 0, 1,                    0, 0, 1, 0, 1, 
0, 0, 0, 1, 0,                    1, 1, 0, 1, 0, 

Grafy G i H po zmianie etykiet wierzchołków
Graf o liczbie 5 wierzchołków     Graf o liczbie 5 wierzchołków
0, 1, 0, 0, 1,                    0, 1, 0, 0, 0, 
1, 0, 1, 0, 0,                    1, 0, 1, 0, 1, 
0, 1, 0, 1, 1,                    0, 1, 0, 1, 0, 
0, 0, 1, 0, 0,                    0, 0, 1, 0, 0, 
1, 0, 1, 0, 0,                    0, 1, 0, 0, 0, 

Rozmiar kliki: 4
Wynik aproksymacyjnego algorytmu: 6, czas wykonania: 0.103 s

===== ALGORYTM APROKSYMACYJNY Z KOLOROWANIEM =====

Grafy G i H przed zmianą etykiet wierzchołków
Graf o liczbie 5 wierzchołków     Graf o liczbie 5 wierzchołków
0, 1, 1, 0, 0,                    0, 0, 0, 0, 1, 
1, 0, 0, 1, 0,                    0, 0, 0, 0, 1, 
1, 0, 0, 1, 0,                    0, 0, 0, 1, 0, 
0, 1, 1, 0, 1,                    0, 0, 1, 0, 1, 
0, 0, 0, 1, 0,                    1, 1, 0, 1, 0, 

Grafy G i H po zmianie etykiet wierzchołków
Graf o liczbie 5 wierzchołków     Graf o liczbie 5 wierzchołków
0, 1, 0, 0, 0,                    0, 1, 0, 1, 1, 
1, 0, 1, 0, 1,                    1, 0, 1, 0, 0, 
0, 1, 0, 1, 0,                    0, 1, 0, 0, 0, 
0, 0, 1, 0, 1,                    1, 0, 0, 0, 0, 
0, 1, 0, 1, 0,                    1, 0, 0, 0, 0, 

Rozmiar kliki: 3
Wynik aproksymacyjnego algorytmu z kolorowaniem: 10, czas wykonania: 0.01 s

===== ALGORYTM DOKŁADNY =====

Grafy G i H przed zmianą etykiet wierzchołków
Graf o liczbie 5 wierzchołków     Graf o liczbie 5 wierzchołków
0, 1, 1, 0, 0,                    0, 0, 0, 0, 1, 
1, 0, 0, 1, 0,                    0, 0, 0, 0, 1, 
1, 0, 0, 1, 0,                    0, 0, 0, 1, 0, 
0, 1, 1, 0, 1,                    0, 0, 1, 0, 1, 
0, 0, 0, 1, 0,                    1, 1, 0, 1, 0, 

Grafy G i H po zmianie etykiet wierzchołków
Graf o liczbie 5 wierzchołków     Graf o liczbie 5 wierzchołków
0, 1, 0, 0, 1,                    0, 1, 0, 0, 0, 
1, 0, 1, 0, 0,                    1, 0, 1, 0, 1, 
0, 1, 0, 1, 1,                    0, 1, 0, 1, 0, 
0, 0, 1, 0, 0,                    0, 0, 1, 0, 0, 
1, 0, 1, 0, 0,                    0, 1, 0, 0, 0, 

Rozmiar kliki: 4
Wynik dokładnego algorytmu: 6, czas wykonania: 0.009 s

Autorzy: Jakub Michalak, Damian Opoka, Wojciech Podmokły
