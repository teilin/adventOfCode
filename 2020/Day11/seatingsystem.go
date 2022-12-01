package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
)

var (
	xd = [...]int{-1, -1, 0, +1, +1, +1, 0, -1}
	yd = [...]int{0, -1, -1, -1, 0, +1, +1, +1}
)

// Seat type
type Seat struct {
	x     int
	y     int
	state rune
}

// Stack type
type Stack []Seat

// IsEmpty check for stack
func (s *Stack) IsEmpty() bool {
	return len(*s) == 0
}

// Push to stack
func (s *Stack) Push(seat Seat) {
	*s = append(*s, seat)
}

// Pop value from stack
func (s *Stack) Pop() (Seat, bool) {
	if s.IsEmpty() {
		return Seat{}, false
	}
	index := len(*s) - 1
	element := (*s)[index]
	*s = (*s)[:index]
	return element, true
}

// CountOccurrences returns the number of a rune occourse in slice
func CountOccurrences(slice *[][]rune, char rune) int {
	count := 0
	for _, row := range *slice {
		for _, seatState := range row {
			if seatState == char {
				count++
			}
		}
	}
	return count
}

func readInput(inputFile string) [][]rune {
	var seatMap [][]rune
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		seatMap = append(seatMap, []rune(scanner.Text()))
	}
	return seatMap
}

func isDirectNeigbourFree(seatMap *[][]rune, seat Seat) bool {
	sidesCheck := 0
	for i := 0; i < len(yd); i++ {
		x := seat.x + xd[i]
		y := seat.y + yd[i]
		if x >= 0 && x < len((*seatMap)[seat.y]) && y >= 0 && y < len(*seatMap) {
			if (*seatMap)[y][x] == 'L' || (*seatMap)[y][x] == '.' {
				sidesCheck++
			}
		} else {
			sidesCheck++
		}
	}
	return sidesCheck == 8
}

func isDirectNeigbourOccupied(seatMap *[][]rune, seat Seat) bool {
	sidesCheck := 0
	for i := 0; i < len(yd); i++ {
		x := seat.x + xd[i]
		y := seat.y + yd[i]
		if x >= 0 && x < len((*seatMap)[seat.y]) && y >= 0 && y < len(*seatMap) {
			if (*seatMap)[y][x] == '#' {
				sidesCheck++
			}
		}
	}
	return sidesCheck >= 4
}

func findSeatInDirection(seatMap *[][]rune, seat Seat) bool {
	sidesCheck := 0
	for i := 0; i < len(yd); i++ {
		c := 1
		for c > 0 {
			x := seat.x + xd[i]*c
			y := seat.y + yd[i]*c
			if x >= 0 && x < len((*seatMap)[seat.y]) && y >= 0 && y < len(*seatMap) {
				if (*seatMap)[y][x] == 'L' || (*seatMap)[y][x] == '.' {
					sidesCheck++
				}
			} else {
				sidesCheck++
			}
		}
	}
	return sidesCheck == 8
}

func checkOccupiedInDirection(seatMap *[][]rune, seat Seat) bool {
	return false
}

func part1(seatMap [][]rune) int {
	var stack Stack
	hasChanged := true
	for hasChanged {
		for y, row := range seatMap {
			for x, seat := range row {
				if seat == 'L' && isDirectNeigbourFree(&seatMap, Seat{x: x, y: y}) {
					stack.Push(Seat{x: x, y: y, state: '#'})
				}
				if seat == '#' && isDirectNeigbourOccupied(&seatMap, Seat{x: x, y: y}) {
					stack.Push(Seat{x: x, y: y, state: 'L'})
				}
			}
		}
		if stack.IsEmpty() {
			hasChanged = false
		}
		for stack.IsEmpty() == false {
			seatChange, _ := stack.Pop()
			seatMap[seatChange.y][seatChange.x] = seatChange.state
		}
	}
	return CountOccurrences(&seatMap, '#')
}

func part2(seatMap [][]rune) int {
	var stack Stack
	hasChanged := true
	for hasChanged {
		for y, row := range seatMap {
			for x, seat := range row {
				if seat == 'L' && findSeatInDirection(&seatMap, Seat{x: x, y: y}) {
					stack.Push(Seat{x: x, y: y, state: '#'})
				}
				if seat == '#' && checkOccupiedInDirection(&seatMap, Seat{x: x, y: y}) {
					stack.Push(Seat{x: x, y: y, state: 'L'})
				}
			}
		}
		if stack.IsEmpty() {
			hasChanged = false
		}
		for stack.IsEmpty() == false {
			seatChange, _ := stack.Pop()
			seatMap[seatChange.y][seatChange.x] = seatChange.state
		}
	}
	return CountOccurrences(&seatMap, '#')
}

func main() {
	var seatMap [][]rune
	seatMap = readInput(os.Args[1])
	numOcupiedSeats := part1(seatMap)
	fmt.Println(numOcupiedSeats)
	seatMap = readInput(os.Args[1])
	numOcupiedSeats = part2(seatMap)
	fmt.Println(numOcupiedSeats)
}
