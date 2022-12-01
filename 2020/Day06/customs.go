package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
)

func countDistinct(array []rune) int {
	counter := make(map[rune]int)
	for _, row := range array {
		counter[row]++
	}
	return len(counter)
}

func readFile(inputString string, isEveryone bool) [][]rune {
	file, err := os.Open(inputString)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	if isEveryone == true {
		var matrix [][]rune
		scanner := bufio.NewScanner(file)
		numPeopleGroup := 0
		counter := make(map[rune]int)
		var group []rune
		for scanner.Scan() {
			s := scanner.Text()
			if s == "" {
				group = make([]rune, 0)
				for a, b := range counter {
					if b == numPeopleGroup {
						group = append(group, a)
					}
				}
				matrix = append(matrix, group)
				counter = make(map[rune]int)
				numPeopleGroup = 0
			} else {
				numPeopleGroup++
			}
			for _, r := range []rune(s) {
				counter[r]++
			}
		}
		group = make([]rune, 0)
		for a, b := range counter {
			if b == numPeopleGroup {
				group = append(group, a)
			}
		}
		matrix = append(matrix, group)
		return matrix
	}
	var matrix [][]rune
	var group []rune
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		s := scanner.Text()
		if s == "" {
			matrix = append(matrix, group)
			group = make([]rune, 0)
		}
		for _, r := range []rune(s) {
			group = append(group, r)
		}
	}
	matrix = append(matrix, group)
	return matrix
}

func part1(matrix [][]rune) int {
	sum := 0
	for _, group := range matrix {
		sum += countDistinct(group)
	}
	return sum
}

func part2(matrix [][]rune) int {
	sum := 0
	for _, group := range matrix {
		sum += countDistinct(group)
	}
	return sum
}

func main() {
	matrix := readFile("input.txt", false)
	sum := part1(matrix)
	fmt.Println("Part 1 sum = " + strconv.Itoa(sum))
	matrix = readFile("input.txt", true)
	sum = part2(matrix)
	fmt.Println("Part 2 sum = " + strconv.Itoa(sum))
}
