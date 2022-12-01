package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"sort"
	"strings"
)

var (
	allNotUnderstood []Ingredient = make([]Ingredient, 0)
	allUnderstood    []Ingredient = make([]Ingredient, 0)
)

// Ingredient type
type Ingredient string

// IngredientList type
type IngredientList struct {
	notunderstand []Ingredient
	understand    []Ingredient
}

func readInput(inputFile string) []IngredientList {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	var ingredientList []IngredientList = make([]IngredientList, 0)
	for scanner.Scan() {
		var list IngredientList
		var tmp []Ingredient = make([]Ingredient, 0)
		lineSplit := strings.Split(scanner.Text(), " (contains ")
		for _, a := range strings.Split(lineSplit[0], " ") {
			tmp = append(tmp, Ingredient(a))
		}
		list.notunderstand = tmp
		tmp = make([]Ingredient, 0)
		for _, a := range strings.Split(lineSplit[1], ", ") {
			lastChar := a[len(a)-1:]
			if lastChar == ")" {
				tmp = append(tmp, Ingredient(a[:len(a)-1]))
			} else {
				tmp = append(tmp, Ingredient(a))
			}
		}
		list.understand = tmp
		tmp = make([]Ingredient, 0)
		ingredientList = append(ingredientList, list)
	}
	return ingredientList
}

func contains(slice []Ingredient, elm Ingredient) bool {
	for _, a := range slice {
		if a == elm {
			return true
		}
	}
	return false
}

func remove(s []Ingredient, i int) []Ingredient {
	copy(s[i:], s[i+1:])
	s[len(s)-1] = ""
	s = s[:len(s)-1]
	return s
}

func findIndex(s []Ingredient, elm Ingredient) int {
	for index, e := range s {
		if elm == e {
			return index
		}
	}
	return len(s)
}

func countElementsWithoutEmpty(s []Ingredient) int {
	count := 0
	for _, e := range s {
		if e != "" {
			count++
		}
	}
	return count
}

func part1(ingredients []IngredientList) (int, map[Ingredient][]Ingredient) {
	for _, i := range ingredients {
		for _, a := range i.notunderstand {
			if contains(allNotUnderstood, a) == false {
				allNotUnderstood = append(allNotUnderstood, a)
			}
		}
		for _, b := range i.understand {
			if contains(allUnderstood, b) == false {
				allUnderstood = append(allUnderstood, b)
			}
		}
	}
	var couldbe map[Ingredient][]Ingredient = make(map[Ingredient][]Ingredient)
	var C map[Ingredient]int = make(map[Ingredient]int)
	for _, cb := range allNotUnderstood {
		var t []Ingredient = make([]Ingredient, 0)
		for _, t1 := range allUnderstood {
			if contains(t, t1) == false {
				t = append(t, t1)
			}
		}
		couldbe[cb] = t
	}
	for _, i := range ingredients {
		for _, i := range i.notunderstand {
			C[i]++
		}
		for _, a := range i.understand {
			for _, b := range allNotUnderstood {
				if contains(i.notunderstand, b) == false {
					s := couldbe[b]
					for n := 0; n < len(s); n++ {
						if s[n] == a {
							remove(s, n)
						}
					}
					couldbe[b] = s
				}
			}
		}
	}
	p1 := 0
	var retIngredients map[Ingredient][]Ingredient = make(map[Ingredient][]Ingredient)
	for _, i := range allNotUnderstood {
		entry := couldbe[i]
		if countElementsWithoutEmpty(entry) == 0 {
			p1 += C[i]
		} else {
			for _, c := range entry {
				if c != "" {
					retIngredients[i] = append(retIngredients[i], c)
				}
			}
		}
	}
	return p1, retIngredients
}

func part2(ingredients map[Ingredient][]Ingredient) string {
	var mapping map[string]Ingredient = make(map[string]Ingredient)
	var used []Ingredient = make([]Ingredient, 0)
	for len(mapping) < len(allUnderstood) {
		for _, i := range allNotUnderstood {
			var poss []Ingredient = make([]Ingredient, 0)
			for _, c := range ingredients[i] {
				if contains(used, c) == false {
					poss = append(poss, c)
				}
				if len(poss) == 1 {
					mapping[string(i)] = poss[0]
					used = append(used, poss[0])
				}
			}
		}
	}
	var tmp []string = make([]string, 0)
	for _, t := range mapping {
		tmp = append(tmp, string(t))
	}
	sort.Strings(tmp)
	retString := ""
	for index, t := range tmp {
		for i, v := range mapping {
			if t == string(v) {
				if index == 0 {
					retString += i
				} else {
					retString += "," + i
				}
			}
		}
	}
	return retString
}

func main() {
	ingredientList := readInput(os.Args[1])
	count, ingredients := part1(ingredientList)
	fmt.Println(count)
	csvList := part2(ingredients)
	fmt.Println(csvList)
}
