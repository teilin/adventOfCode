package main

import (
	"bufio"
	"fmt"
	"log"
	"math"
	"os"
	"strconv"
)

// Navigation type
type Navigation struct {
	action rune
	value  float64
}

// Coordinate type
type Coordinate struct {
	x float64
	y float64
}

func readInput(inoutFile string) []Navigation {
	file, err := os.Open(inoutFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	var inputs []Navigation
	for scanner.Scan() {
		line := scanner.Text()
		v, _ := strconv.Atoi(line[1:])
		inputs = append(inputs, Navigation{action: []rune(line[0:1])[0], value: float64(v)})
	}
	return inputs
}

func part1(navigation []Navigation) float64 {
	currentPosition := Coordinate{x: 0, y: 0}
	heading := 90
	for _, nav := range navigation {
		if nav.action == 'E' {
			currentPosition.x += nav.value
		} else if nav.action == 'W' {
			currentPosition.x -= nav.value
		} else if nav.action == 'N' {
			currentPosition.y += nav.value
		} else if nav.action == 'S' {
			currentPosition.y -= nav.value
		} else if nav.action == 'L' {
			heading = (heading - int(nav.value)) % 360
		} else if nav.action == 'R' {
			heading = (heading + int(nav.value)) % 360
		} else if nav.action == 'F' {
			if heading == 90 {
				currentPosition.x += nav.value
			} else if heading == 180 {
				currentPosition.y -= nav.value
			} else if heading == 270 {
				currentPosition.x -= nav.value
			} else if heading == 0 || heading == 360 {
				currentPosition.x += nav.value
			}
		}
	}
	return math.Abs(currentPosition.x) + math.Abs(currentPosition.y)
}

func main() {
	inputData := readInput(os.Args[1])
	distance := part1(inputData)
	fmt.Println(distance)
}
