package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"math"
	"os"
	"strconv"
	"strings"
)

func readInput(inputString string) (int, []int) {
	file, err := ioutil.ReadFile(inputString)
	if err != nil {
		log.Fatal(err)
	}
	tmp := strings.Split(string(file), "\n")
	myTimeStamp, _ := strconv.Atoi(tmp[0])
	var busIDs []int
	for _, busID := range strings.Split(tmp[1], ",") {
		if busID != "x" {
			c, _ := strconv.Atoi(busID)
			busIDs = append(busIDs, c)
		}
	}
	return myTimeStamp, busIDs
}

func readInput2(inputString string) []string {
	file, err := ioutil.ReadFile(inputString)
	if err != nil {
		log.Fatal(err)
	}
	tmp := strings.Split(string(file), "\n")
	var busIDs []string
	for _, busID := range strings.Split(tmp[1], ",") {
		busIDs = append(busIDs, busID)
	}
	return busIDs
}

func part1(earliestTimestamp int, busIDs []int) (int, int) {
	index := 1
	earliestBusID := 0
	earliestBusIDTime := 0
	for _, busID := range busIDs {
		for index > 0 {
			if busID*index >= earliestTimestamp {
				if earliestBusID == 0 || busID*index <= earliestBusIDTime {
					earliestBusID = busID
					earliestBusIDTime = busID * index
				}
				break
			}
			index++
		}
		index = 1
	}
	return earliestBusID, earliestBusIDTime
}

func modPower(b, e, mod int64) int64 {
	if e == 0 {
		return 1
	} else if e%2 == 0 {
		return modPower((b*b)%mod, e/2, mod)
	}
	return (b * modPower(b, e-1, mod)) % mod
}

func modInverse(a, m int64) int64 {
	m0 := m
	var y int64 = 0
	var x int64 = 1
	if m == 1 {
		return 0
	}
	for a > 1 {
		q := math.Floor(float64(a) / float64(m))
		t := m
		m = a % m
		a = t
		t = y
		y = x - int64(q)*y
		x = t
	}
	if x < 0 {
		x = x + m0
	}
	return x
}

func part2(busList []string) int64 {
	var busMap map[int64]int64 = make(map[int64]int64)
	var N int64 = 1
	for i, bus := range busList {
		if bus != "x" {
			b, _ := strconv.Atoi(bus)
			busMap[int64((b-i+1)%b)] = int64(b)
			N *= int64(b)
		}
	}
	var ans int64 = 0
	for i, b := range busMap {
		var ni int64 = N / b
		mi := modInverse(ni, b)
		var forB int64 = i * mi * ni
		ans += forB
	}
	return ans%N - 1
}

func main() {
	earliestTimestamp, busIDs := readInput(os.Args[1])
	busID, time := part1(earliestTimestamp, busIDs)
	minutesWait := time - earliestTimestamp
	fmt.Println("Earliest busid: " + strconv.Itoa(busID) + " at time " + strconv.Itoa(time) + ". Multiplied: " + strconv.Itoa(busID*minutesWait))
	busList := readInput2(os.Args[1])
	firstDeparture := part2(busList)
	fmt.Println(firstDeparture)
}
