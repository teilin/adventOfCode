package main

import (
	"bufio"
	"fmt"
	"log"
	"math"
	"os"
	"reflect"
	"strconv"
	"strings"
)

func toMask(mask string) []rune {
	tmp := []rune(mask)
	reverseAny(tmp)
	return tmp
}

func toBinary(value string) []int {
	tmp := []rune(value)
	var slice []int = make([]int, len(tmp))
	for i := len(tmp) - 1; i >= 0; i-- {
		if tmp[i] == '0' {
			slice[i] = 0
		} else {
			slice[i] = 1
		}
	}
	reverseAny(slice)
	return slice
}

func reverseAny(s interface{}) {
	n := reflect.ValueOf(s).Len()
	swap := reflect.Swapper(s)
	for i, j := 0, n-1; i < j; i, j = i+1, j-1 {
		swap(i, j)
	}
}

func reverse(s string) string {
	runes := []rune(s)
	for i, j := 0, len(runes)-1; i < j; i, j = i+1, j-1 {
		runes[i], runes[j] = runes[j], runes[i]
	}
	return string(runes)
}

func fromBinary(slice []int) int {
	tmp := ""
	for _, i := range slice {
		tmp += strconv.Itoa(i)
	}
	tmp = reverse(tmp)
	value, _ := strconv.ParseInt(tmp, 2, 64)
	return int(value)
}

func add(value int, mask string) int {
	v := toBinary(strconv.FormatInt(int64(value), 2))
	m := toMask(mask)
	var r []int = make([]int, 36)
	for i := 0; i < len(m); i++ {
		if i < len(v) {
			if m[i] == 'X' {
				r[i] = v[i]
			}
			if m[i] != 'X' {
				if m[i] == '0' {
					r[i] = 0
				} else {
					r[i] = 1
				}
			}
		} else {
			if m[i] != 'X' {
				if m[i] == '0' {
					r[i] = 0
				} else {
					r[i] = 1
				}
			} else {
				r[i] = 0
			}
		}
	}
	return fromBinary(r)
}

func getMemAddress(memAddr int, mask string) []int {
	a := toBinary(strconv.FormatInt(int64(memAddr), 2))
	m := toMask(mask)
	var r []rune = make([]rune, 36)
	for i := 0; i < len(m); i++ {
		if i < len(a) {
			if m[i] == '0' {
				r[i] = []rune(strconv.Itoa(a[i]))[0]
			}
			if m[i] == '1' {
				r[i] = '1'
			}
			if m[i] == 'X' {
				r[i] = 'X'
			}
		} else {
			if m[i] == '1' {
				r[i] = '1'
			} else if m[i] == 'X' {
				r[i] = 'X'
			} else {
				r[i] = '0'
			}
		}
	}
	return getAll(r)
}

func countFloatingBits(slice []rune) int {
	count := 0
	for _, c := range slice {
		if c == 'X' {
			count++
		}
	}
	return count
}

func replaceFloating(address []rune, v []int) []int {
	var arr []int = make([]int, len(address))
	j := 0
	for i, r := range address {
		if r == 'X' {
			arr[i] = v[j]
			j++
		}
		if r == '0' {
			arr[i] = 0
		}
		if r == '1' {
			arr[i] = 1
		}
	}
	return arr
}

func getAll(floatingBits []rune) []int {
	var ret []int = make([]int, 0)

	numFloating := countFloatingBits(floatingBits)
	for _, variation := range getVariations(numFloating) {
		tmp := replaceFloating(floatingBits, variation)
		decimalVal := fromBinary(tmp)
		ret = append(ret, decimalVal)
	}
	return ret
}

func sumMap(memory map[string]int) int {
	sum := 0
	for _, v := range memory {
		sum += v
	}
	return sum
}

func sumMap2(memory map[int]int) int {
	sum := 0
	for _, v := range memory {
		sum += v
	}
	return sum
}

func runProgram(inputFile string) int {
	var memory map[string]int = make(map[string]int)
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	mask := ""
	mem := ""
	for scanner.Scan() {
		line := scanner.Text()
		if strings.Contains(line, "mask") {
			mask = strings.Split(line, " = ")[1]
		}
		if strings.Contains(line, "mem") {
			mem = strings.Split(line, " = ")[0]
			value, _ := strconv.Atoi(strings.Split(line, " = ")[1])
			memory[mem] = add(value, mask)
		}
	}
	return sumMap(memory)
}

func part1(inputFile string) int {
	return runProgram(inputFile)
}

func part2(inputFile string) int {
	var memory map[int]int = make(map[int]int)
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	mask := ""
	mem := 0
	for scanner.Scan() {
		line := scanner.Text()
		if strings.Contains(line, "mask") {
			mask = strings.Split(line, " = ")[1]
		}
		if strings.Contains(line, "mem") {
			str := strings.ReplaceAll(strings.Split(line, " = ")[0], "mem[", "")
			memAddr, _ := strconv.Atoi(str[:len(str)-1])
			mem = memAddr
			value, _ := strconv.Atoi(strings.Split(line, " = ")[1])
			addresses := getMemAddress(mem, mask)
			for _, addr := range addresses {
				memory[addr] = value
			}
		}
	}
	return sumMap2(memory)
}

func getVariations(n int) [][]int {
	var arr [][]int = make([][]int, 0)
	for i := 0; i <= int(math.Pow(2, float64(n))-1); i++ {
		var row []int = make([]int, n)
		binary := strconv.FormatInt(int64(i), 2)
		r := []rune(binary)
		len := len(r)
		index := 0
		for _, x := range r {
			for len < n {
				row[index] = 0
				len++
				index++
			}
			t, _ := strconv.Atoi(string(x))
			row[index] = t
			index++
		}
		arr = append(arr, row)
	}
	return arr
}

func main() {
	sum := part1(os.Args[1])
	fmt.Println(sum)
	sum = part2(os.Args[1])
	fmt.Println(sum)
}
