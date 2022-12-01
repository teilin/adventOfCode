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

func readInput(adapters *[]int, inputFile string) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line, _ := strconv.Atoi(strings.TrimSpace(scanner.Text()))
		*adapters = append(*adapters, line)
	}
	sort.IntSlice.Sort(*adapters)
}

func part1(adapters *[]int) (int, int) {
	latestJolt := 0
	diff1 := 0
	diff3 := 0
	for _, jolt := range *adapters {
		if jolt-latestJolt > 0 && jolt-latestJolt <= 3 {
			if jolt-latestJolt == 1 {
				diff1++
			} else if jolt-latestJolt == 3 {
				diff3++
			}
			latestJolt = jolt
		}
	}
	return diff1, diff3
}

func contains(slice *[]int, num int) bool {
	for _, val := range *slice {
		if val == num {
			return true
		}
	}
	return false
}

func findAll(adapters *[]int, alternatives *[][]int) {
	*alternatives = append(*alternatives, []int{0})
	for index := 0; index < len(*alternatives); index++ {
		lastJolt := (*alternatives)[index][len((*alternatives)[index])-1]
		numFound := 0
		for _, jolt := range *adapters {
			lastJolt = (*alternatives)[index][len((*alternatives)[index])-1]
			if jolt-lastJolt > 0 && jolt-lastJolt <= 3 {
				numFound++
				(*alternatives)[index] = append((*alternatives)[index], jolt)
				if numFound > 1 {
					newAlt := (*alternatives)[index]
					newAlt = append(newAlt, jolt)
					*alternatives = append(*alternatives, newAlt)
				}
			} else {
				numFound = 0
			}
		}
	}
}

func part2(adapters *[]int) int64 {
	return 0
}

func main() {
	var adapters []int
	readInput(&adapters, os.Args[1])
	adapters = append(adapters, adapters[len(adapters)-1]+3)
	diff1, diff3 := part1(&adapters)
	fmt.Println(diff1 * diff3) // 1998
	numDistinct := part2(&adapters)
	fmt.Println(numDistinct) // 347250213298688
}
