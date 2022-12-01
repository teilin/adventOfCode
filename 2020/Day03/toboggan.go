package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
)

// Defining global const variables
const (
	TREE rune = rune('#')
)

func readInput(inputFile string) [][]rune {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	var matrix [][]rune
	index := 0
	for scanner.Scan() {
		matrix = append(matrix, []rune(scanner.Text()))
		index++
	}
	return matrix
}

func calculateCollisions(roadMap [][]rune, rightMove int, downMove int) int {
	treeHits := 0
	maxY := len(roadMap)
	maxX := len(roadMap[0])
	curPosX := 0
	curPosY := 0
	for curPosY < maxY {
		curPosX += rightMove
		curPosY += downMove
		if curPosY < maxY {
			if roadMap[curPosY][curPosX%maxX] == TREE {
				treeHits++
			}
		}
	}
	return treeHits
}

func main() {
	roadMap := readInput("map.txt")

	right1down1 := calculateCollisions(roadMap, 1, 1)
	right3down1 := calculateCollisions(roadMap, 3, 1)
	right5down1 := calculateCollisions(roadMap, 5, 1)
	right7down1 := calculateCollisions(roadMap, 7, 1)
	right1down2 := calculateCollisions(roadMap, 1, 2)

	fmt.Println("Solution part 1 = " + strconv.Itoa(right3down1))
	part2 := right3down1 * right1down1 * right5down1 * right7down1 * right1down2
	fmt.Println("Solution part 2 = " + strconv.Itoa(part2))
}
