use std::{env, fs::read_to_string};

fn main() {
    let args: Vec<String> = env::args().collect();

    if args.len() != 3 {
        eprintln!("usage: cargo run {{filename}} {{part}}");

        return;
    }

    let lines: Vec<String> = read_to_string(&args[1])
        .unwrap()
        .lines()
        .map(String::from)
        .collect();

    match args[2].as_str() {
        "one" => part_one(lines),
        "two" => part_two(lines),
        _ => unreachable!(),
    }
}

fn part_one(lines: Vec<String>) {
    let mut total: usize = 0;

    for line in lines {
        let splitted = line
            .split(' ')
            .map(|n| n.parse::<usize>().unwrap())
            .collect::<Vec<_>>();

        if verify(&splitted) {
            total += 1;
        }
    }

    println!("{total}");
}

fn part_two(lines: Vec<String>) {
    let mut total: usize = 0;

    for line in lines {
        let splitted = line
            .split(' ')
            .map(|n| n.parse::<usize>().unwrap())
            .collect::<Vec<_>>();

        if verify(&splitted) {
            total += 1;
            continue;
        }

        let mut valid = false;

        for i in 0..splitted.len() {
            let mut vec = splitted.clone();
            vec.remove(i);

            if verify(&vec) {
                valid = true;
                break;
            }
        }

        if valid {
            total += 1;
        }
    }

    println!("{total}");
}

fn verify(items: &[usize]) -> bool {
    let mut should_increase: Option<bool> = None;

    for pair in items.windows(2) {
        if !is_valid(pair[0], pair[1], &mut should_increase) {
            return false;
        }
    }

    true
}

fn is_valid(left: usize, right: usize, should_increase: &mut Option<bool>) -> bool {
    let difference = usize::abs_diff(left, right);

    if difference > 3 || difference == 0 {
        return false;
    }

    let is_increasing = left < right;

    if should_increase.is_none() {
        *should_increase = Some(is_increasing);
    }

    if let Some(should_increase) = &should_increase {
        if is_increasing != *should_increase {
            return false;
        }
    }

    true
}
