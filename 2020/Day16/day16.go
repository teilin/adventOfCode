package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
	"time"

	"github.com/gnboorse/centipede"
)

// Range type
type Range struct {
	min int
	max int
}

// Rule type
type Rule struct {
	name   string
	range1 Range
	range2 Range
}

func readInput(inutFile string) ([]Rule, []int, [][]int) {
	file, err := os.Open(inutFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	var rules []Rule = make([]Rule, 0)
	var myTicket []int = make([]int, 0)
	var nearbyTickets [][]int = make([][]int, 0)
	section := 1
	for scanner.Scan() {
		line := scanner.Text()
		if line != "" {
			if line == "your ticket:" {
				section = 2
			}
			if line == "nearby tickets:" {
				section = 3
			}
			if section == 1 {
				parts := strings.Split(line, ": ")
				tmp := strings.Split(parts[1], " or ")
				t := strings.Split(tmp[0], "-")
				min, _ := strconv.Atoi(t[0])
				max, _ := strconv.Atoi(t[1])
				p1 := Range{min: min, max: max}
				t = strings.Split(tmp[1], "-")
				min, _ = strconv.Atoi(t[0])
				max, _ = strconv.Atoi(t[1])
				p2 := Range{min: min, max: max}
				r := Rule{name: parts[0], range1: p1, range2: p2}
				rules = append(rules, r)
			} else if section == 2 && line != "your ticket:" {
				parts := strings.Split(line, ",")
				myTicket = make([]int, len(parts))
				for index, v := range parts {
					i, _ := strconv.Atoi(v)
					myTicket[index] = i
				}
			} else if section == 3 && line != "nearby tickets:" {
				parts := strings.Split(line, ",")
				var ticket []int = make([]int, len(parts))
				for index, v := range parts {
					i, _ := strconv.Atoi(v)
					ticket[index] = i
				}
				nearbyTickets = append(nearbyTickets, ticket)
			}
		}
	}
	return rules, myTicket, nearbyTickets
}

func sum(array []int) int {
	s := 0
	for _, i := range array {
		s += i
	}
	return s
}

func part1(rules []Rule, nearbyTickets [][]int) (int, [][]int) {
	var scanningError []int = make([]int, 0)
	var validTickets [][]int = make([][]int, 0)
	for _, ticket := range nearbyTickets {
		isValid := true
		for _, num := range ticket {
			validFor := len(rules)
			for _, rule := range rules {
				if !((num >= rule.range1.min && num <= rule.range1.max) || (num >= rule.range2.min && num <= rule.range2.max)) {
					validFor--
				}
			}
			if validFor < 1 {
				scanningError = append(scanningError, num)
				isValid = false
			}
		}
		if isValid {
			validTickets = append(validTickets, ticket)
		}
	}
	return sum(scanningError), validTickets
}

func getFreePositions(rulePositions map[string]int, numField int) []int {
	var freePositions []int = make([]int, 0)
	for i := 1; i <= numField; i++ {
		isFree := true
		for _, p := range rulePositions {
			if p == i {
				isFree = false
			}
		}
		if isFree {
			freePositions = append(freePositions, i)
		}
	}
	return freePositions
}

func determineFieldPosition(rules []Rule, nearbyValidTickets [][]int) map[string]int {
	var rulePosition map[string]int = make(map[string]int)
	var posMap map[int]int = make(map[int]int)
	var used map[int]bool = make(map[int]bool)
	var ok [][]bool = make([][]bool, 0)

	for _, validTickets := range nearbyValidTickets {
		var isOk []bool = make([]bool, len(validTickets))
		for index, element := range validTickets {
			for ruleIndex, rule := range rules {

			}
		}
	}

	return rulePosition
}

func part2(rules []Rule, myTicket []int, nearbyValidTickets [][]int) int {
	rulePosition := determineFieldPosition(rules, nearbyValidTickets)
	num := 1
	for rule, position := range rulePosition {
		if strings.HasPrefix(rule, "departure") {
			num *= myTicket[position]
		}
	}
	return num
}

func part2CSP() {
	vars := centipede.Variables{
		centipede.NewVariable("", centipede.IntRange(0, 1)),
	}
	constraints := centipede.Constraints{
		centipede.Equals("", ""),
	}
	solver := centipede.NewBackTrackingCSPSolver(vars, constraints)
	begin := time.Now()
	success := solver.Solve()
	elapsed := time.Since(begin)
	if success {
		fmt.Printf("Found solution in %s\n", elapsed)
		for _, variable := range solver.State.Vars {
			fmt.Printf("Variable %v = %v\n", variable.Name, variable.Value)
		}
	} else {
		fmt.Println("Could not find a solution")
	}
}

func main() {
	rules, myTicket, nearbyTickets := readInput(os.Args[1])
	ticketScanningErrorRate, validNearbyTickets := part1(rules, nearbyTickets)
	fmt.Println(ticketScanningErrorRate)
	multiple := part2(rules, myTicket, validNearbyTickets)
	fmt.Println(multiple)
}
