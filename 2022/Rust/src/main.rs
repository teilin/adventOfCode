use std::env;

mod day01;

fn main() {
    let args: Vec<String> = env::args().collect();
    let file_input = &args[1];
    day01::part1(file_input);
}