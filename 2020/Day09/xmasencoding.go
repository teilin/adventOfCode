package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"sort"
	"strconv"
	"strings"
)

func readInput(encodingErrors *[]int, inputFile string) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		number, _ := strconv.Atoi(strings.TrimSpace(scanner.Text()))
		*encodingErrors = append(*encodingErrors, number)
	}
}

func findXMASError(encodingError *[]int, preamble int) int {
	for i := preamble; i < len(*encodingError); i++ {
		minIndex := i - preamble
		maxIndex := i - 1
		foundSum := false
		for a := minIndex; a <= maxIndex; a++ {
			for b := maxIndex; b >= minIndex; b-- {
				if a != b {
					if (*encodingError)[i] == (*encodingError)[a]+(*encodingError)[b] {
						foundSum = true
						break
					}
				}
			}
			if foundSum {
				break
			}
		}
		if foundSum == false {
			return (*encodingError)[i]
		}
	}
	return -1
}

func continguesSet(encodingError *[]int, invalidNumber int) int {
	var contSet []int
	sum := 0
	for a := 0; a < len(*encodingError); a++ {
		contSet = make([]int, 0)
		sum = 0
		for b := a; b < len(*encodingError); b++ {
			contSet = append(contSet, (*encodingError)[b])
			sum += (*encodingError)[b]
			if sum >= invalidNumber {
				break
			}
		}
		if sum == invalidNumber {
			break
		}
	}
	sort.IntSlice.Sort(contSet)
	return contSet[0] + contSet[len(contSet)-1]
}

func main() {
	var encodingErrors []int
	readInput(&encodingErrors, "input.txt")
	invalidNumber := findXMASError(&encodingErrors, 25)
	fmt.Println(invalidNumber)
	sumMinMaxSet := continguesSet(&encodingErrors, invalidNumber)
	fmt.Println(sumMinMaxSet)
}
