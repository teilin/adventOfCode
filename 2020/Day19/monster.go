package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

var (
	rules map[RuleNumber]Rule = make(map[RuleNumber]Rule)
)

// RuleNumber type
type RuleNumber int

// RuleSet type
type RuleSet []RuleNumber

// Rule type
type Rule struct {
	part1 RuleSet
	part2 RuleSet
	exp   rune
}

func readInput(inputFile string) [][]rune {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	isRules := true
	var programInput [][]rune = make([][]rune, 0)
	for scanner.Scan() {
		line := scanner.Text()
		if line != "" {
			if isRules {
				rule := Rule{}
				r := strings.Split(line, ": ")
				ruleNo, _ := strconv.Atoi(r[0])
				r2 := strings.Split(r[1], " | ")
				p1 := strings.Split(r2[0], " ")
				part1 := RuleSet{}
				part2 := RuleSet{}
				for _, v := range p1 {
					t, _ := strconv.Atoi(v)
					part1 = append(part1, RuleNumber(t))
				}
				if len(r2) == 2 {
					for _, v := range p1 {
						t, _ := strconv.Atoi(v)
						part2 = append(part2, RuleNumber(t))
					}
				}
				rule.part1 = part1
				rule.part2 = part2
				rules[RuleNumber(ruleNo)] = rule
			} else {
				programInput = append(programInput, []rune(line))
			}
		} else {
			isRules = false
		}
	}
	return programInput
}

func matchChar(rule Rule, char rune) bool {
	part1, part2 := true, true
	if len(rule.part1) > 0 {
		for _, p1 := range rule.part1 {
			r := rules[p1]
			part1 = matchChar(r, char)
		}
	} else {
		part1 = rule.exp == char
	}
	if len(rule.part2) > 0 {
		for _, p2 := range rule.part2 {
			r := rules[p2]
			part2 = matchChar(r, char)
		}
	} else {
		part2 = rule.exp == char
	}
	return part1 || part2
}

func matchRule(ruleNo RuleNumber, message []rune) bool {
	rule := rules[ruleNo]
	for _, char := range message {
		if matchChar(rule, char) == false {
			return false
		}
	}
	return true
}

func part1(programInput [][]rune) int {
	count := 0
	for _, message := range programInput {
		if matchRule(RuleNumber(0), message) {
			count++
		}
	}
	return count
}

func main() {
	programInput := readInput(os.Args[1])
	numMessagesMatchRule0 := part1(programInput)
	fmt.Println(numMessagesMatchRule0)
}
