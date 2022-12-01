package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
)

var (
	spaceMap map[Coordinate]Active = make(map[Coordinate]Active)
)

// Coordinate type
type Coordinate struct {
	x int
	y int
	z int
}

// Active type
type Active bool

// Range function
func Range(start int, end int) []int {
	var r []int = make([]int, (end-start)+1)
	index := 0
	for i := start; i <= end; i++ {
		r[index] = i
		index++
	}
	return r
}

func readInput(inputFile string) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	y := 0
	for scanner.Scan() {
		runes := []rune(scanner.Text())
		for x := 0; x < len(runes); x++ {
			coor := Coordinate{x: x, y: y, z: 0}
			if runes[x] == '#' {
				spaceMap[coor] = Active(true)
			} else {
				spaceMap[coor] = Active(false)
			}
		}
		y++
	}
}

func allNeighbours(cell Coordinate) []Coordinate {
	var allNeighbours []Coordinate = make([]Coordinate, 0)
	for _, x1 := range Range(cell.x-1, cell.x+1) {
		for _, y1 := range Range(cell.y-1, cell.y+1) {
			for _, z1 := range Range(cell.z-1, cell.z+1) {
				c := Coordinate{x: x1, y: y1, z: z1}
				if c != cell {
					allNeighbours = append(allNeighbours, c)
				}
			}
		}
	}
	return allNeighbours
}

func countActiveCells() int {
	count := 0
	for _, cellState := range spaceMap {
		if cellState == true {
			count++
		}
	}
	return count
}

func part1() int {
	for cell, state := range spaceMap {
		for _, neighbour := range allNeighbours(cell) {
			fmt.Println(state)
			test := spaceMap[neighbour]
			fmt.Println(test)
		}
	}
	return countActiveCells()
}

func main() {
	readInput(os.Args[1])
	cubesActive := part1()
	fmt.Println(cubesActive)
}
