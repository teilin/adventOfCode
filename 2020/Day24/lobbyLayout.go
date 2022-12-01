package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
)

// Coordinate type
type Coordinate struct {
	x int
	y int
	z int
}

// Stack type
type Stack []Coordinate

// Coor type
type Coor struct {
	x int
	y int
}

// IsEmpty check for stack
func (s *Stack) IsEmpty() bool {
	return len(*s) == 0
}

// Push to stack
func (s *Stack) Push(coor Coordinate) {
	*s = append(*s, coor)
}

// Pop value from stack
func (s *Stack) Pop() (Coordinate, bool) {
	if s.IsEmpty() {
		return Coordinate{}, false
	}
	index := len(*s) - 1
	element := (*s)[index]
	*s = (*s)[:index]
	return element, true
}

func readInput(inputFile string) []string {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	lobbyTiles := make([]string, 0)
	for scanner.Scan() {
		lobbyTiles = append(lobbyTiles, scanner.Text())
	}
	return lobbyTiles
}

func directionToCoordinate(dir string) (Coordinate, bool, string) {
	coor := Coordinate{x: 0, y: 0}
	retString := ""
	if dir[:1] == "e" {
		coor.x = 1
		coor.y = -1
		retString = dir[1:]
	} else if dir[:1] == "w" {
		coor.x = -1
		coor.y = 1
		retString = dir[1:]
	} else if dir[:2] == "se" {
		coor.z = 1
		coor.y = -1
		retString = dir[2:]
	} else if dir[:2] == "sw" {
		coor.z = 1
		coor.x = -1
		retString = dir[2:]
	} else if dir[:2] == "nw" {
		coor.z = -1
		coor.y = 1
		retString = dir[2:]
	} else if dir[:2] == "ne" {
		coor.x = 1
		coor.z = -1
		retString = dir[2:]
	} else {
		return coor, false, ""
	}
	return coor, true, retString
}

// Add function
func (t *Coordinate) Add(move Coordinate) Coordinate {
	(*t).x += move.x
	(*t).y += move.y
	return *t
}

func countBlackTiles(tilesLayout map[Coordinate]bool) int {
	count := 0
	for _, t := range tilesLayout {
		if t {
			count++
		}
	}
	return count
}

func part1(lobbyTiles []string) (int, map[Coordinate]bool) {
	var tiles map[Coordinate]bool = make(map[Coordinate]bool)
	for _, tile := range lobbyTiles {
		c := Coordinate{x: 0, y: 0}
		retString := tile
		for retString != "" {
			c2, exists, retString2 := directionToCoordinate(retString)
			if exists {
				retString = retString2
				c = c.Add(c2)
			}
		}
		f := tiles[c]
		tiles[c] = !f
	}
	count := countBlackTiles(tiles)
	return count, tiles
}

// (1,-1,0),(0,-1,1),(-1,0,1),(-1,1,0),(0,1,-1),(1,0,-1)
func getDirections() []Coordinate {
	dir := make([]Coordinate, 6)
	dir[0] = Coordinate{x: 1, y: -1, z: 0}
	dir[1] = Coordinate{x: 0, y: -1, z: 1}
	dir[2] = Coordinate{x: -1, y: 0, z: 1}
	dir[3] = Coordinate{x: -1, y: 1, z: 0}
	dir[4] = Coordinate{x: 0, y: 1, z: -1}
	dir[5] = Coordinate{x: 1, y: 0, z: -1}
	return dir
}

func part2(tiles map[Coordinate]bool) int {
	counter := 0
	var stack Stack
	for counter < 100 {
		check := make([]Coordinate, 0)
		for coor, _ := range tiles {
			check = append(check, coor)
			for _, c := range getDirections() {
				check = append(check, coor.Add(c))
			}
		}
		for _, coor := range check {
			nbr := 0
			for _, dxdydz := range getDirections() {
				if tiles[coor.Add(dxdydz)] {
					nbr++
				}
			}
			if tiles[coor] && (nbr == 1 || nbr == 2) {
				stack.Push(coor)
			}
			if tiles[coor] == false && nbr == 2 {
				stack.Push(coor)
			}
		}
		// Apply changes
		for stack.IsEmpty() == false {
			c, e := stack.Pop()
			if e {
				tmp := tiles[c]
				tiles[c] = !tmp
			}
		}
		counter++
	}
	return countBlackTiles(tiles)
}

func main() {
	lobbyTiles := readInput(os.Args[1])
	numBlackTiles, tilesLayout := part1(lobbyTiles)
	fmt.Println(numBlackTiles)
	numBlackTiles = part2(tilesLayout)
	fmt.Println(numBlackTiles)
}
