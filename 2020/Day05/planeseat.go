package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
)

// Defining constants
const (
	maxRows    = 127
	maxColumns = 7
)

func readFile(inputFile string) []string {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	var array []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		array = append(array, scanner.Text())
	}
	return array
}

func calculateSeat(space []rune) (int, int) {
	minRow := 0
	maxRow := maxRows
	minColumn := 0
	maxColumn := maxColumns
	for _, code := range space {
		if code == rune('F') {
			maxRow = (minRow + maxRow) / 2
		} else if code == rune('B') {
			minRow = (minRow + maxRow) / 2
		} else if code == rune('R') {
			minColumn = (minColumn + maxColumn) / 2
		} else if code == rune('L') {
			maxColumn = (minColumn + maxColumn) / 2
		}
	}
	return maxRow, maxColumn
}

func part1(inputFile string) int {
	maxSeatID := 0

	planeSeats := readFile(inputFile)

	for _, seatCodes := range planeSeats {
		row, column := calculateSeat([]rune(seatCodes))
		seatID := row*8 + column
		//fmt.Println("SeatID = " + strconv.Itoa(seatID))
		if seatID > maxSeatID {
			maxSeatID = seatID
		}
	}

	return maxSeatID
}

func part2(inputFile string) int {
	seatMap := make([][]bool, maxRows+1)
	for index := range seatMap {
		seatMap[index] = make([]bool, maxColumns+1)
		for innerIndex := range seatMap[index] {
			seatMap[index][innerIndex] = false
		}
	}
	boardingPasses := readFile(inputFile)
	for _, boardingPass := range boardingPasses { // boardingPass
		row, column := calculateSeat([]rune(boardingPass))
		if row > maxRows || column > maxColumns || row < 0 || column < 0 {
			fmt.Println("Damn, error")
		}
		if seatMap[row][column] == false {
			seatMap[row][column] = true
		} else {
			fmt.Println("Something wrong? This seat is already marked taken...")
		}
	}
	for rowIndex := range seatMap {
		for columnIndex := range seatMap[rowIndex] {
			if rowIndex > 0 && rowIndex < len(seatMap) {
				if columnIndex > 0 && columnIndex < len(seatMap[rowIndex]) {
					if seatMap[rowIndex][columnIndex] == false {
						if seatMap[rowIndex][columnIndex+1] == true && seatMap[rowIndex][columnIndex-1] == true {
							fmt.Println("My seat: Row = " + strconv.Itoa(rowIndex) + " - Column = " + strconv.Itoa(columnIndex))
							return rowIndex*8 + columnIndex
						}
					}
				}
			}
		}
	}
	return 0
}

func main() {
	maxSeatID := part1("planeseats.txt")
	fmt.Println("Max seatID = " + strconv.Itoa(maxSeatID))
	mySeatID := part2("planeseats.txt")
	fmt.Println("My seatID = " + strconv.Itoa(mySeatID))
}
