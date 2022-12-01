package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
)

func readLines(path string) ([]int, error) {
	file, err := os.Open(path)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	var lines []int
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		num, _ := strconv.Atoi(scanner.Text())
		lines = append(lines, num)
	}
	return lines, scanner.Err()
}

func part1(numbers []int) int {
	for _, x := range numbers {
		for _, y := range numbers {
			if x+y == 2020 {
				return x * y
				break
			}
		}
	}
	return -1
}

func part2(numbers []int) int {
	for _, x := range numbers {
		for _, y := range numbers {
			for _, z := range numbers {
				if x+y+z == 2020 {
					return x * y * z
				}
			}
		}
	}
	return -1
}

func main() {
	numbers, err := readLines("part1input.txt")
	if err != nil {
		log.Fatal(err)
	}
	fmt.Println("Part 1")
	firstSolution := part1(numbers)
	fmt.Println(firstSolution)

	fmt.Println("Part 2")
	secondSolution := part2(numbers)
	fmt.Println(secondSolution)
}
